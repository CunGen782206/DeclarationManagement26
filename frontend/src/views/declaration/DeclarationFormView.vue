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
import type { AttachmentItem, FlowLogItem, ReviewRecordItem, TaskItem } from '@/types/domain';
import { awardLevelOptions, categoryOptions, departmentOptions, participationTypeOptions, projectLevelOptions } from '@/utils/constants';

const route = useRoute();
const router = useRouter();

const declarationId = computed(() => Number(route.params.id || 0));
const tasks = ref<TaskItem[]>([]);
const attachments = ref<AttachmentItem[]>([]);
const reviewRecords = ref<ReviewRecordItem[]>([]);
const flowLogs = ref<FlowLogItem[]>([]);

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
  if (!declarationId.value) {
    return;
  }

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
    const res = await createDeclarationApi(form);
    ElMessage.success('创建成功');
    router.replace(`/declarations/${res.data.data}`);
    return;
  }

  await updateDeclarationApi(declarationId.value, form);
  ElMessage.success('保存成功');
};

const submit = async () => {
  if (!declarationId.value) {
    return;
  }

  await submitDeclarationApi(declarationId.value);
  ElMessage.success('提交成功');
};

const resubmit = async () => {
  if (!declarationId.value) {
    return;
  }

  await resubmitDeclarationApi(declarationId.value);
  ElMessage.success('重新提交成功');
};

const onUploadChange = async (uploadFile: { raw?: File }) => {
  if (!declarationId.value || !uploadFile.raw) {
    return;
  }

  await uploadAttachmentApi(declarationId.value, uploadFile.raw);
  ElMessage.success('附件上传成功');
  await loadExtraData();
};

const downloadAttachment = async (attachment: AttachmentItem) => {
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

  if (!declarationId.value) {
    return;
  }

  const res = await getDeclarationDetailApi(declarationId.value);
  Object.assign(form, res.data.data);
  await loadExtraData();
});
</script>

<template>
  <div class="page-container">
    <el-card>
      <template #header>申报表单</template>
      <el-form label-width="120px">
        <el-row :gutter="16">
          <el-col :span="12">
            <el-form-item label="申报任务">
              <el-select v-model="form.taskId">
                <el-option v-for="task in tasks" :key="task.id" :label="task.taskName" :value="task.id" />
              </el-select>
            </el-form-item>
          </el-col>
          <el-col :span="12">
            <el-form-item label="负责人">
              <el-input v-model="form.principalName" />
            </el-form-item>
          </el-col>
          <el-col :span="12">
            <el-form-item label="联系方式">
              <el-input v-model="form.contactPhone" />
            </el-form-item>
          </el-col>
          <el-col :span="12">
            <el-form-item label="所属部门">
              <el-select v-model="form.departmentId">
                <el-option v-for="item in departmentOptions" :key="item.id" :label="item.name" :value="item.id" />
              </el-select>
            </el-form-item>
          </el-col>
          <el-col :span="12">
            <el-form-item label="项目名称">
              <el-input v-model="form.projectName" />
            </el-form-item>
          </el-col>
          <el-col :span="12">
            <el-form-item label="项目类别">
              <el-select v-model="form.projectCategoryId">
                <el-option v-for="item in categoryOptions" :key="item.id" :label="item.name" :value="item.id" />
              </el-select>
            </el-form-item>
          </el-col>
          <el-col :span="12">
            <el-form-item label="项目等级">
              <el-select v-model="form.projectLevel">
                <el-option v-for="item in projectLevelOptions" :key="item.value" :label="item.label" :value="item.value" />
              </el-select>
            </el-form-item>
          </el-col>
          <el-col :span="12">
            <el-form-item label="奖项级别">
              <el-select v-model="form.awardLevel">
                <el-option v-for="item in awardLevelOptions" :key="item.value" :label="item.label" :value="item.value" />
              </el-select>
            </el-form-item>
          </el-col>
          <el-col :span="12">
            <el-form-item label="参与形式">
              <el-select v-model="form.participationType">
                <el-option v-for="item in participationTypeOptions" :key="item.value" :label="item.label" :value="item.value" />
              </el-select>
            </el-form-item>
          </el-col>
          <el-col :span="12">
            <el-form-item label="认定批文名称">
              <el-input v-model="form.approvalDocumentName" />
            </el-form-item>
          </el-col>
          <el-col :span="12">
            <el-form-item label="盖章单位及时间">
              <el-input v-model="form.sealUnitAndDate" />
            </el-form-item>
          </el-col>
          <el-col :span="24">
            <el-form-item label="项目内容">
              <el-input v-model="form.projectContent" type="textarea" :rows="4" />
            </el-form-item>
          </el-col>
          <el-col :span="24">
            <el-form-item label="项目成果">
              <el-input v-model="form.projectAchievement" type="textarea" :rows="4" />
            </el-form-item>
          </el-col>
        </el-row>
      </el-form>

      <div class="toolbar">
        <el-button type="primary" @click="save">保存</el-button>
        <el-button type="success" :disabled="!declarationId" @click="submit">提交</el-button>
        <el-button :disabled="!declarationId" @click="resubmit">驳回后重提</el-button>
      </div>
    </el-card>

    <el-card v-if="declarationId" style="margin-top: 16px">
      <template #header>附件上传下载</template>
      <el-upload :auto-upload="false" :show-file-list="false" :on-change="onUploadChange">
        <el-button type="primary">上传附件</el-button>
      </el-upload>

      <el-table :data="attachments" border style="margin-top: 12px">
        <el-table-column prop="originalFileName" label="文件名" />
        <el-table-column prop="fileSizeBytes" label="大小(Byte)" width="120" />
        <el-table-column prop="uploadedAt" label="上传时间" width="180" />
        <el-table-column label="操作" width="120">
          <template #default="{ row }">
            <el-button link type="primary" @click="downloadAttachment(row)">下载</el-button>
          </template>
        </el-table-column>
      </el-table>
    </el-card>

    <el-card v-if="declarationId" style="margin-top: 16px">
      <template #header>审核历史与流程日志</template>

      <h4>审核记录</h4>
      <el-table :data="reviewRecords" border>
        <el-table-column prop="reviewStage" label="阶段" width="100" />
        <el-table-column prop="reviewAction" label="动作" width="100" />
        <el-table-column prop="reason" label="原因" />
        <el-table-column prop="recognizedAmount" label="认定金额" width="120" />
        <el-table-column prop="remark" label="备注" />
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
