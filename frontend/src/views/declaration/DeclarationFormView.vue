<script setup lang="ts">
import { computed, onMounted, onUnmounted, reactive, ref } from 'vue';
import { onBeforeRouteLeave, useRoute, useRouter } from 'vue-router';
import { ElMessage } from 'element-plus';
import { Close } from '@element-plus/icons-vue';
import {
  clearTemporaryAttachmentsApi,
  getAttachmentsApi,
  getDeclarationDetailApi,
  getTemporaryAttachmentsApi,
  submitDeclarationApi,
  uploadAttachmentApi,
  uploadTemporaryAttachmentApi
} from '@/api/declaration';
import { getReviewRecordsApi } from '@/api/review';
import { listTasksApi } from '@/api/task';
import { DeclarationStatus, ReviewAction, ReviewStage } from '@/types/common';
import type { AttachmentItem, DeclarationDetail, ReviewRecordItem, TaskItem } from '@/types/domain';
import { awardLevelOptions, categoryOptions, departmentOptions, participationTypeOptions, projectLevelOptions } from '@/utils/constants';

const route = useRoute();
const router = useRouter();

const declarationId = computed(() => Number(route.params.id || 0));
const routeMode = computed(() => String(route.query.mode || ''));
const tasks = ref<TaskItem[]>([]);
const attachments = ref<AttachmentItem[]>([]);
const reviewRecords = ref<ReviewRecordItem[]>([]);
const detail = ref<DeclarationDetail | null>(null);
const submitting = ref(false);
const uploading = ref(false);
const tempAttachmentKey = ref(globalThis.crypto?.randomUUID?.() ?? `${Date.now()}-${Math.random().toString(16).slice(2)}`);
const shouldCleanupTemporaryAttachments = ref(true);

const form = reactive({
  taskId: undefined as number | undefined,
  principalName: '',
  contactPhone: '',
  departmentId: undefined as number | undefined,
  projectName: '',
  projectCategoryId: undefined as number | undefined,
  projectLevel: 1,
  awardLevel: 1,
  participationType: 1,
  approvalDocumentName: '',
  sealUnitAndDate: '',
  projectContent: '',
  projectAchievement: ''
});

const isNew = computed(() => !declarationId.value);
const isRejectedDeclaration = computed(() => detail.value?.currentStatus === DeclarationStatus.PreReviewRejected
  || detail.value?.currentStatus === DeclarationStatus.InitialReviewRejected
  || detail.value?.currentStatus === DeclarationStatus.Draft);
const isViewMode = computed(() => !isNew.value && (routeMode.value === 'view' || !isRejectedDeclaration.value));
const canSubmit = computed(() => isNew.value || (!isViewMode.value && isRejectedDeclaration.value));
const canUploadAttachment = computed(() => isNew.value || (!isViewMode.value && isRejectedDeclaration.value));
const showReviewRecords = computed(() => Boolean(declarationId.value && detail.value?.submittedAt));
const pageTitle = computed(() => {
  if (isNew.value) {
    return '新建申报';
  }

  return isViewMode.value ? '查看申报' : '修改申报';
});

const reviewStageLabel = (stage: ReviewStage) => stage === ReviewStage.PreReview ? '部门预审' : '初审';
const reviewActionLabel = (action: ReviewAction) => ({ 1: '通过', 2: '不通过', 3: '驳回' }[action] ?? '');

const assignForm = (source: DeclarationDetail) => {
  form.taskId = source.taskId;
  form.principalName = source.principalName;
  form.contactPhone = source.contactPhone;
  form.departmentId = source.departmentId;
  form.projectName = source.projectName;
  form.projectCategoryId = source.projectCategoryId;
  form.projectLevel = source.projectLevel;
  form.awardLevel = source.awardLevel;
  form.participationType = source.participationType;
  form.approvalDocumentName = source.approvalDocumentName ?? '';
  form.sealUnitAndDate = source.sealUnitAndDate ?? '';
  form.projectContent = source.projectContent ?? '';
  form.projectAchievement = source.projectAchievement ?? '';
};

const loadAttachments = async () => {
  if (isNew.value) {
    const res = await getTemporaryAttachmentsApi(tempAttachmentKey.value);
    attachments.value = res.data.data;
    return;
  }

  const res = await getAttachmentsApi(declarationId.value);
  attachments.value = res.data.data;
};

const loadReviewRecords = async () => {
  if (!showReviewRecords.value) {
    reviewRecords.value = [];
    return;
  }

  const res = await getReviewRecordsApi(declarationId.value);
  reviewRecords.value = res.data.data;
};

const loadPage = async () => {
  const taskRes = await listTasksApi();
  tasks.value = taskRes.data.data;

  if (isNew.value) {
    await loadAttachments();
    return;
  }

  const res = await getDeclarationDetailApi(declarationId.value);
  detail.value = res.data.data;
  assignForm(detail.value);
  await Promise.all([loadAttachments(), loadReviewRecords()]);
};

const validateForm = () => {
  if (!form.taskId || !form.departmentId || !form.projectCategoryId) {
    ElMessage.warning('请先完善必填项');
    return false;
  }

  if (!form.projectName.trim() || !form.principalName.trim() || !form.contactPhone.trim()) {
    ElMessage.warning('请先完善必填项');
    return false;
  }

  return true;
};

const submit = async () => {
  if (!canSubmit.value || !validateForm()) {
    return;
  }

  submitting.value = true;
  try {
    await submitDeclarationApi({
      declarationId: declarationId.value || undefined,
      taskId: form.taskId,
      principalName: form.principalName,
      contactPhone: form.contactPhone,
      departmentId: form.departmentId,
      projectName: form.projectName,
      projectCategoryId: form.projectCategoryId,
      projectLevel: form.projectLevel,
      awardLevel: form.awardLevel,
      participationType: form.participationType,
      approvalDocumentName: form.approvalDocumentName,
      sealUnitAndDate: form.sealUnitAndDate,
      projectContent: form.projectContent,
      projectAchievement: form.projectAchievement,
      tempAttachmentKey: isNew.value ? tempAttachmentKey.value : undefined
    });

    shouldCleanupTemporaryAttachments.value = false;
    ElMessage.success('提交成功');
    await router.replace('/declarations');
  } finally {
    submitting.value = false;
  }
};

const handleUpload = async (file: File) => {
  if (!canUploadAttachment.value) {
    return false;
  }

  uploading.value = true;
  try {
    if (isNew.value) {
      await uploadTemporaryAttachmentApi(tempAttachmentKey.value, file);
    } else {
      await uploadAttachmentApi(declarationId.value, file);
    }

    ElMessage.success('附件上传成功');
    await loadAttachments();
  } finally {
    uploading.value = false;
  }

  return false;
};

const cleanupTemporaryAttachments = async () => {
  if (!isNew.value || !shouldCleanupTemporaryAttachments.value || attachments.value.length === 0) {
    return;
  }

  await clearTemporaryAttachmentsApi(tempAttachmentKey.value);
  attachments.value = [];
};

const cleanupOnWindowUnload = () => {
  if (!isNew.value || !shouldCleanupTemporaryAttachments.value || attachments.value.length === 0) {
    return;
  }

  const baseUrl = (import.meta.env.VITE_API_BASE_URL ?? '/api').replace(/\/$/, '');
  const token = localStorage.getItem('token');
  const endpoint = `${baseUrl}/declarations/temp-attachments?tempAttachmentKey=${encodeURIComponent(tempAttachmentKey.value)}`;

  void fetch(endpoint, {
    method: 'DELETE',
    keepalive: true,
    headers: token ? { Authorization: `Bearer ${token}` } : undefined
  });
};

const closePage = async () => {
  await cleanupTemporaryAttachments();
  await router.push('/declarations');
};

onBeforeRouteLeave(async () => {
  await cleanupTemporaryAttachments();
});

onMounted(async () => {
  window.addEventListener('beforeunload', cleanupOnWindowUnload);
  await loadPage();
});

onUnmounted(() => {
  window.removeEventListener('beforeunload', cleanupOnWindowUnload);
});
</script>

<template>
  <div class="page-container">
    <el-card>
      <template #header>
        <div style="display: flex; align-items: center; justify-content: space-between">
          <span>{{ pageTitle }}</span>
          <el-button circle text @click="closePage">
            <el-icon><Close /></el-icon>
          </el-button>
        </div>
      </template>

      <el-form label-width="120px">
        <el-row :gutter="16">
          <el-col :span="12">
            <el-form-item label="申报任务">
              <el-select v-model="form.taskId" :disabled="isViewMode">
                <el-option v-for="task in tasks" :key="task.id" :label="task.taskName" :value="task.id" />
              </el-select>
            </el-form-item>
          </el-col>
          <el-col :span="12">
            <el-form-item label="负责人">
              <el-input v-model="form.principalName" :disabled="isViewMode" />
            </el-form-item>
          </el-col>
          <el-col :span="12">
            <el-form-item label="联系方式">
              <el-input v-model="form.contactPhone" :disabled="isViewMode" />
            </el-form-item>
          </el-col>
          <el-col :span="12">
            <el-form-item label="所属部门">
              <el-select v-model="form.departmentId" :disabled="isViewMode">
                <el-option v-for="item in departmentOptions" :key="item.id" :label="item.name" :value="item.id" />
              </el-select>
            </el-form-item>
          </el-col>
          <el-col :span="12">
            <el-form-item label="项目名称">
              <el-input v-model="form.projectName" :disabled="isViewMode" />
            </el-form-item>
          </el-col>
          <el-col :span="12">
            <el-form-item label="项目类别">
              <el-select v-model="form.projectCategoryId" :disabled="isViewMode">
                <el-option v-for="item in categoryOptions" :key="item.id" :label="item.name" :value="item.id" />
              </el-select>
            </el-form-item>
          </el-col>
          <el-col :span="12">
            <el-form-item label="项目等级">
              <el-select v-model="form.projectLevel" :disabled="isViewMode">
                <el-option v-for="item in projectLevelOptions" :key="item.value" :label="item.label" :value="item.value" />
              </el-select>
            </el-form-item>
          </el-col>
          <el-col :span="12">
            <el-form-item label="获奖级别">
              <el-select v-model="form.awardLevel" :disabled="isViewMode">
                <el-option v-for="item in awardLevelOptions" :key="item.value" :label="item.label" :value="item.value" />
              </el-select>
            </el-form-item>
          </el-col>
          <el-col :span="12">
            <el-form-item label="参与形式">
              <el-select v-model="form.participationType" :disabled="isViewMode">
                <el-option v-for="item in participationTypeOptions" :key="item.value" :label="item.label" :value="item.value" />
              </el-select>
            </el-form-item>
          </el-col>
          <el-col :span="12">
            <el-form-item label="认定批文名称">
              <el-input v-model="form.approvalDocumentName" :disabled="isViewMode" />
            </el-form-item>
          </el-col>
          <el-col :span="12">
            <el-form-item label="盖章单位及时间">
              <el-input v-model="form.sealUnitAndDate" :disabled="isViewMode" />
            </el-form-item>
          </el-col>
          <el-col :span="24">
            <el-form-item label="项目内容">
              <el-input v-model="form.projectContent" type="textarea" :rows="4" :disabled="isViewMode" />
            </el-form-item>
          </el-col>
          <el-col :span="24">
            <el-form-item label="项目成果">
              <el-input v-model="form.projectAchievement" type="textarea" :rows="4" :disabled="isViewMode" />
            </el-form-item>
          </el-col>
        </el-row>
      </el-form>

      <div class="toolbar">
        <el-button type="success" :loading="submitting" @click="submit" v-if="canSubmit">提交</el-button>
      </div>
    </el-card>

    <el-card style="margin-top: 16px">
      <template #header>附件上传</template>

      <el-upload :show-file-list="false" :before-upload="handleUpload" v-if="canUploadAttachment">
        <el-button type="primary" :loading="uploading">上传附件</el-button>
      </el-upload>

      <el-table :data="attachments" border style="margin-top: 12px">
        <el-table-column prop="originalFileName" label="文件名" />
        <el-table-column prop="fileSizeBytes" label="大小(Byte)" width="140" />
        <el-table-column prop="uploadedAt" label="上传时间" width="180" />
      </el-table>
    </el-card>

    <el-card v-if="showReviewRecords" style="margin-top: 16px">
      <template #header>审核记录</template>
      <el-table :data="reviewRecords" border>
        <el-table-column label="阶段" width="100">
          <template #default="{ row }">{{ reviewStageLabel(row.reviewStage) }}</template>
        </el-table-column>
        <el-table-column label="动作" width="100">
          <template #default="{ row }">{{ reviewActionLabel(row.reviewAction) }}</template>
        </el-table-column>
        <el-table-column prop="reason" label="原因" />
        <el-table-column prop="recognizedAmount" label="认定金额" width="120" />
        <el-table-column prop="remark" label="备注" />
        <el-table-column prop="reviewedAt" label="审核时间" width="180" />
      </el-table>
    </el-card>
  </div>
</template>
