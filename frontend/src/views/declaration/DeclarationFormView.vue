<script setup lang="ts">
import { computed, onMounted, reactive, ref } from 'vue';
import { useRoute, useRouter } from 'vue-router';
import { ElMessage } from 'element-plus';
import {
  createDeclarationApi,
  downloadAttachmentApi,
  getAttachmentsApi,
  getDeclarationDetailApi,
  resubmitDeclarationApi,
  submitDeclarationApi,
  updateDeclarationApi,
  uploadAttachmentApi
} from '@/api/declaration';
import { getFlowLogsApi, getReviewRecordsApi } from '@/api/review';
import { listTasksApi } from '@/api/task';
import { awardLevelOptions, categoryOptions, departmentOptions, participationTypeOptions, projectLevelOptions } from '@/utils/constants';

const route = useRoute();
const router = useRouter();
const declarationId = computed(() => Number(route.params.id || 0));
const tasks = ref<{ id: number; taskName: string }[]>([]);
const attachments = ref<any[]>([]);
const reviewRecords = ref<any[]>([]);
const flowLogs = ref<any[]>([]);

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

const loadExtraData = async () => {
  if (!declarationId.value) return;
  const [attachmentRes, recordsRes, logsRes] = await Promise.all([
    getAttachmentsApi(declarationId.value),
    getReviewRecordsApi(declarationId.value),
    getFlowLogsApi(declarationId.value)
  ]);
  attachments.value = attachmentRes.data.data;
  reviewRecords.value = recordsRes.data.data;
  flowLogs.value = logsRes.data.data;
};

const save = async () => {
  if (!declarationId.value) {
    const res = await createDeclarationApi(form as unknown as Record<string, unknown>);
    ElMessage.success('创建成功');
    router.replace(`/declarations/${res.data.data}`);
    return;
  }
  await updateDeclarationApi(declarationId.value, form as unknown as Record<string, unknown>);
  ElMessage.success('保存成功');
};

const submit = async () => {
  if (!declarationId.value) return;
  await submitDeclarationApi(declarationId.value);
  ElMessage.success('提交成功');
};

const resubmit = async () => {
  if (!declarationId.value) return;
  await resubmitDeclarationApi(declarationId.value);
  ElMessage.success('重提成功');
};

const onUploadChange = async (file: any) => {
  if (!declarationId.value || !file.raw) return;
  await uploadAttachmentApi(declarationId.value, file.raw as File);
  ElMessage.success('附件上传成功');
  await loadExtraData();
};

const downloadAttachment = async (attachment: any) => {
  const res = await downloadAttachmentApi(attachment.id);
  const url = window.URL.createObjectURL(res.data);
  const link = document.createElement('a');
  link.href = url;
  link.download = attachment.originalFileName;
  link.click();
  window.URL.revokeObjectURL(url);
};

onMounted(async () => {
  const taskRes = await listTasksApi();
  tasks.value = taskRes.data.data;
  if (declarationId.value) {
    const res = await getDeclarationDetailApi(declarationId.value);
    Object.assign(form, res.data.data);
    await loadExtraData();
  }
});
</script>

<template>
  <div class="page-container">
    <el-card>
      <template #header>申报表单</template>
      <el-form label-width="120px">
        <el-row :gutter="16">
          <el-col :span="12"><el-form-item label="申报任务"><el-select v-model="form.taskId"><el-option v-for="t in tasks" :key="t.id" :value="t.id" :label="t.taskName" /></el-select></el-form-item></el-col>
          <el-col :span="12"><el-form-item label="负责人"><el-input v-model="form.principalName" /></el-form-item></el-col>
          <el-col :span="12"><el-form-item label="联系方式"><el-input v-model="form.contactPhone" /></el-form-item></el-col>
          <el-col :span="12"><el-form-item label="所属部门"><el-select v-model="form.departmentId"><el-option v-for="i in departmentOptions" :key="i.id" :value="i.id" :label="i.name" /></el-select></el-form-item></el-col>
          <el-col :span="12"><el-form-item label="项目名称"><el-input v-model="form.projectName" /></el-form-item></el-col>
          <el-col :span="12"><el-form-item label="项目类别"><el-select v-model="form.projectCategoryId"><el-option v-for="i in categoryOptions" :key="i.id" :value="i.id" :label="i.name" /></el-select></el-form-item></el-col>
          <el-col :span="12"><el-form-item label="项目等级"><el-select v-model="form.projectLevel"><el-option v-for="i in projectLevelOptions" :key="i.value" :value="i.value" :label="i.label" /></el-select></el-form-item></el-col>
          <el-col :span="12"><el-form-item label="奖项级别"><el-select v-model="form.awardLevel"><el-option v-for="i in awardLevelOptions" :key="i.value" :value="i.value" :label="i.label" /></el-select></el-form-item></el-col>
          <el-col :span="12"><el-form-item label="参与形式"><el-select v-model="form.participationType"><el-option v-for="i in participationTypeOptions" :key="i.value" :value="i.value" :label="i.label" /></el-select></el-form-item></el-col>
          <el-col :span="12"><el-form-item label="认定批文名称"><el-input v-model="form.approvalDocumentName" /></el-form-item></el-col>
          <el-col :span="12"><el-form-item label="盖章单位及时间"><el-input v-model="form.sealUnitAndDate" /></el-form-item></el-col>
          <el-col :span="24"><el-form-item label="项目内容"><el-input type="textarea" v-model="form.projectContent" /></el-form-item></el-col>
          <el-col :span="24"><el-form-item label="项目成果"><el-input type="textarea" v-model="form.projectAchievement" /></el-form-item></el-col>
        </el-row>
      </el-form>
      <div class="toolbar">
        <el-button type="primary" @click="save">保存</el-button>
        <el-button type="success" @click="submit" :disabled="!declarationId">提交</el-button>
        <el-button @click="resubmit" :disabled="!declarationId">驳回后重提</el-button>
      </div>
    </el-card>

    <el-card style="margin-top: 16px" v-if="declarationId">
      <template #header>附件上传下载</template>
      <el-upload :auto-upload="false" :show-file-list="false" :on-change="onUploadChange">
        <el-button type="primary">上传附件</el-button>
      </el-upload>
      <el-table :data="attachments" border style="margin-top: 12px">
        <el-table-column prop="originalFileName" label="文件名" />
        <el-table-column prop="fileSizeBytes" label="大小(Byte)" width="120" />
        <el-table-column prop="uploadedAt" label="上传时间" width="180" />
        <el-table-column label="操作" width="120">
          <template #default="scope">
            <el-button type="primary" link @click="downloadAttachment(scope.row)">下载</el-button>
          </template>
        </el-table-column>
      </el-table>
    </el-card>

    <el-card style="margin-top: 16px" v-if="declarationId">
      <template #header>审核历史与流程日志</template>
      <h4>审核记录</h4>
      <el-table :data="reviewRecords" border>
        <el-table-column prop="reviewStage" label="阶段" width="100" />
        <el-table-column prop="reviewAction" label="动作" width="100" />
        <el-table-column prop="reason" label="原因" />
        <el-table-column prop="recognizedAmount" label="认定金额" width="120" />
        <el-table-column prop="reviewedAt" label="审核时间" width="180" />
      </el-table>

      <h4 style="margin-top: 16px">流程日志</h4>
      <el-table :data="flowLogs" border>
        <el-table-column prop="fromStatus" label="来源状态" width="120" />
        <el-table-column prop="toStatus" label="目标状态" width="120" />
        <el-table-column prop="actionType" label="动作类型" width="120" />
        <el-table-column prop="note" label="备注" />
        <el-table-column prop="createdAt" label="时间" width="180" />
      </el-table>
    </el-card>
  </div>
</template>
