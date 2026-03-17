<script setup lang="ts">
import { onMounted, reactive, ref } from 'vue';
import { ElMessage } from 'element-plus';
import { getFlowLogsApi, getPendingReviewsApi, getReviewRecordsApi, reviewActionApi } from '@/api/review';
import { DeclarationStatus, ReviewAction, ReviewStage } from '@/types/common';
import type { FlowLogItem, PendingReviewItem, ReviewRecordItem } from '@/types/domain';
import { awardLevelOptions, categoryOptions, declarationStatusOptions, departmentOptions, projectLevelOptions } from '@/utils/constants';

const query = reactive({
  pageIndex: 1,
  pageSize: 10,
  startDate: '',
  endDate: '',
  projectName: '',
  departmentIds: [] as number[],
  categoryIds: [] as number[],
  statuses: [DeclarationStatus.PendingPreReview, DeclarationStatus.PendingInitialReview] as number[]
});

const rows = ref<PendingReviewItem[]>([]);
const total = ref(0);
const actionDialogVisible = ref(false);
const historyDialogVisible = ref(false);
const records = ref<ReviewRecordItem[]>([]);
const flowLogs = ref<FlowLogItem[]>([]);

const actionForm = reactive({
  declarationId: 0,
  reviewStage: ReviewStage.PreReview,
  reviewAction: ReviewAction.Pass,
  reason: '',
  recognizedProjectLevel: 1,
  recognizedAwardLevel: 1,
  recognizedAmount: 0,
  remark: ''
});

const loadData = async () => {
  const res = await getPendingReviewsApi(query);
  rows.value = res.data.data.items;
  total.value = res.data.data.totalCount;
};

const openAction = (row: PendingReviewItem) => {
  actionForm.declarationId = row.declarationId;
  actionForm.reviewStage = row.currentReviewStage;
  actionForm.reviewAction = ReviewAction.Pass;
  actionForm.reason = '';
  actionForm.recognizedProjectLevel = 1;
  actionForm.recognizedAwardLevel = 1;
  actionForm.recognizedAmount = 0;
  actionForm.remark = '';
  actionDialogVisible.value = true;
};

const openHistory = async (row: PendingReviewItem) => {
  const [recordsRes, logsRes] = await Promise.all([
    getReviewRecordsApi(row.declarationId),
    getFlowLogsApi(row.declarationId)
  ]);
  records.value = recordsRes.data.data;
  flowLogs.value = logsRes.data.data;
  historyDialogVisible.value = true;
};

const submitReview = async () => {
  await reviewActionApi(actionForm);
  ElMessage.success('审核提交成功');
  actionDialogVisible.value = false;
  await loadData();
};

const reviewStageLabel = (stage: ReviewStage) => stage === ReviewStage.PreReview ? '部门预审' : '初审';
const reviewActionLabel = (action: ReviewAction) => ({ 1: '通过', 2: '不通过', 3: '驳回' }[action] ?? '');

onMounted(loadData);
</script>

<template>
  <div class="page-container">
    <div class="toolbar" style="display: grid; grid-template-columns: repeat(4, minmax(200px, 1fr)); gap: 12px">
      <el-date-picker v-model="query.startDate" type="date" placeholder="开始日期" value-format="YYYY-MM-DD" />
      <el-date-picker v-model="query.endDate" type="date" placeholder="结束日期" value-format="YYYY-MM-DD" />
      <el-input v-model="query.projectName" placeholder="项目名称" />
      <el-select v-model="query.departmentIds" multiple collapse-tags placeholder="所属部门">
        <el-option v-for="item in departmentOptions" :key="item.id" :label="item.name" :value="item.id" />
      </el-select>
      <el-select v-model="query.categoryIds" multiple collapse-tags placeholder="项目类别">
        <el-option v-for="item in categoryOptions" :key="item.id" :label="item.name" :value="item.id" />
      </el-select>
      <el-select v-model="query.statuses" multiple collapse-tags placeholder="状态">
        <el-option v-for="item in declarationStatusOptions" :key="item.value" :label="item.label" :value="item.value" />
      </el-select>
      <div style="display: flex; gap: 8px">
        <el-button type="primary" @click="loadData">查询</el-button>
      </div>
    </div>

    <el-table :data="rows" border style="margin-top: 16px">
      <el-table-column prop="projectName" label="项目名称" />
      <el-table-column prop="projectCategoryName" label="项目类别" />
      <el-table-column prop="departmentName" label="所属部门" />
      <el-table-column label="当前审核阶段" width="120">
        <template #default="{ row }">
          {{ reviewStageLabel(row.currentReviewStage) }}
        </template>
      </el-table-column>
      <el-table-column prop="currentStatus" label="状态" width="120" />
      <el-table-column prop="submittedAt" label="提交时间" width="180" />
      <el-table-column label="操作" width="220">
        <template #default="{ row }">
          <el-button type="primary" link @click="openAction(row)">审核</el-button>
          <el-button type="info" link @click="openHistory(row)">查看历史</el-button>
        </template>
      </el-table-column>
    </el-table>

    <el-pagination
      v-model:current-page="query.pageIndex"
      v-model:page-size="query.pageSize"
      style="margin-top: 16px"
      :total="total"
      layout="total, prev, pager, next, jumper"
      @current-change="loadData"
    />

    <el-dialog v-model="actionDialogVisible" title="执行审核" width="680px">
      <el-form label-width="120px">
        <el-form-item label="审核阶段">
          <el-input :model-value="reviewStageLabel(actionForm.reviewStage)" disabled />
        </el-form-item>
        <el-form-item label="审核动作">
          <el-radio-group v-model="actionForm.reviewAction">
            <el-radio :value="1">通过</el-radio>
            <el-radio :value="2">不通过</el-radio>
            <el-radio :value="3">驳回</el-radio>
          </el-radio-group>
        </el-form-item>
        <el-form-item label="原因">
          <el-input v-model="actionForm.reason" type="textarea" :rows="3" />
        </el-form-item>
        <el-form-item v-if="actionForm.reviewAction === ReviewAction.Pass" label="认定项目等级">
          <el-select v-model="actionForm.recognizedProjectLevel">
            <el-option v-for="item in projectLevelOptions" :key="item.value" :label="item.label" :value="item.value" />
          </el-select>
        </el-form-item>
        <el-form-item v-if="actionForm.reviewAction === ReviewAction.Pass" label="认定奖项级别">
          <el-select v-model="actionForm.recognizedAwardLevel">
            <el-option v-for="item in awardLevelOptions" :key="item.value" :label="item.label" :value="item.value" />
          </el-select>
        </el-form-item>
        <el-form-item v-if="actionForm.reviewAction === ReviewAction.Pass" label="认定金额">
          <el-input-number v-model="actionForm.recognizedAmount" :min="0" />
        </el-form-item>
        <el-form-item label="备注">
          <el-input v-model="actionForm.remark" type="textarea" :rows="3" />
        </el-form-item>
      </el-form>
      <template #footer>
        <el-button @click="actionDialogVisible = false">取消</el-button>
        <el-button type="primary" @click="submitReview">提交</el-button>
      </template>
    </el-dialog>

    <el-dialog v-model="historyDialogVisible" title="审核历史与流程日志" width="960px">
      <h4>审核记录</h4>
      <el-table :data="records" border>
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

      <h4 style="margin-top: 16px">流程日志</h4>
      <el-table :data="flowLogs" border>
        <el-table-column prop="fromStatus" label="来源状态" width="120" />
        <el-table-column prop="toStatus" label="目标状态" width="120" />
        <el-table-column prop="actionType" label="动作类型" width="120" />
        <el-table-column prop="note" label="备注" />
        <el-table-column prop="createdAt" label="时间" width="180" />
      </el-table>
    </el-dialog>
  </div>
</template>
