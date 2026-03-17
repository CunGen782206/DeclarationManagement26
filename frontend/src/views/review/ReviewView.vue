<script setup lang="ts">
import { onMounted, reactive, ref } from 'vue';
import { ElMessage } from 'element-plus';
import { getFlowLogsApi, getPendingReviewsApi, getReviewRecordsApi, reviewActionApi } from '@/api/review';
import { ReviewAction } from '@/types/common';

const query = reactive({ pageIndex: 1, pageSize: 10 });
const rows = ref<any[]>([]);
const total = ref(0);
const actionDialogVisible = ref(false);
const historyDialogVisible = ref(false);
const records = ref<any[]>([]);
const flowLogs = ref<any[]>([]);
const actionForm = reactive({ declarationId: 0, action: ReviewAction.Pass, reason: '', recognizedProjectLevel: 1, recognizedAwardLevel: 1, recognizedAmount: 0, remark: '' });

const loadData = async () => {
  const res = await getPendingReviewsApi(query);
  rows.value = res.data.data.items;
  total.value = res.data.data.totalCount;
};

const openAction = (row: any) => {
  actionForm.declarationId = row.declarationId;
  actionForm.action = ReviewAction.Pass;
  actionForm.reason = '';
  actionDialogVisible.value = true;
};

const openHistory = async (row: any) => {
  const declarationId = row.declarationId;
  const [recordsRes, logsRes] = await Promise.all([
    getReviewRecordsApi(declarationId),
    getFlowLogsApi(declarationId)
  ]);
  records.value = recordsRes.data.data;
  flowLogs.value = logsRes.data.data;
  historyDialogVisible.value = true;
};

const submitReview = async () => {
  await reviewActionApi(actionForm as unknown as Record<string, unknown>);
  ElMessage.success('审核提交成功');
  actionDialogVisible.value = false;
  await loadData();
};

onMounted(loadData);
</script>

<template>
  <div class="page-container">
    <el-table :data="rows" border>
      <el-table-column prop="projectName" label="项目名称" />
      <el-table-column prop="departmentName" label="所属部门" />
      <el-table-column prop="currentReviewStage" label="当前审核阶段" />
      <el-table-column prop="submittedAt" label="提交时间" />
      <el-table-column label="操作" width="220">
        <template #default="scope">
          <el-button type="primary" link @click="openAction(scope.row)">审核</el-button>
          <el-button type="info" link @click="openHistory(scope.row)">查看历史</el-button>
        </template>
      </el-table-column>
    </el-table>
    <el-pagination style="margin-top:16px" v-model:current-page="query.pageIndex" v-model:page-size="query.pageSize" :total="total" @current-change="loadData" />

    <el-dialog v-model="actionDialogVisible" title="执行审核" width="640px">
      <el-form label-width="120px">
        <el-form-item label="审核动作">
          <el-radio-group v-model="actionForm.action">
            <el-radio :value="1">通过</el-radio>
            <el-radio :value="2">不通过</el-radio>
            <el-radio :value="3">驳回</el-radio>
          </el-radio-group>
        </el-form-item>
        <el-form-item label="原因"><el-input v-model="actionForm.reason" /></el-form-item>
        <el-form-item label="认定金额"><el-input-number v-model="actionForm.recognizedAmount" :min="0" /></el-form-item>
        <el-form-item label="备注"><el-input type="textarea" v-model="actionForm.remark" /></el-form-item>
      </el-form>
      <template #footer><el-button @click="actionDialogVisible=false">取消</el-button><el-button type="primary" @click="submitReview">提交</el-button></template>
    </el-dialog>

    <el-dialog v-model="historyDialogVisible" title="审核历史与流程日志" width="900px">
      <h4>审核记录</h4>
      <el-table :data="records" border>
        <el-table-column prop="reviewStage" label="阶段" width="100" />
        <el-table-column prop="reviewAction" label="动作" width="100" />
        <el-table-column prop="reason" label="原因" />
        <el-table-column prop="recognizedAmount" label="认定金额" width="120" />
        <el-table-column prop="reviewedAt" label="审核时间" width="180" />
      </el-table>
      <h4 style="margin-top:16px">流程日志</h4>
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
