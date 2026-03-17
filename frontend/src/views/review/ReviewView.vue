<script setup lang="ts">
import { onMounted, reactive, ref } from 'vue';
import { ElMessage } from 'element-plus';
import { getPendingReviewsApi, reviewActionApi } from '@/api/review';
import { ReviewAction } from '@/types/common';

const query = reactive({ pageIndex: 1, pageSize: 10 });
const rows = ref<any[]>([]);
const total = ref(0);
const actionDialogVisible = ref(false);
const actionForm = reactive({ declarationId: 0, action: ReviewAction.Pass, reason: '', recognizedProjectLevel: 1, recognizedAwardLevel: 1, recognizedAmount: 0, remark: '' });

const loadData = async () => {
  const res = await getPendingReviewsApi(query);
  rows.value = res.data.data.items;
  total.value = res.data.data.totalCount;
};

const openAction = (row: any) => {
  actionForm.declarationId = row.id;
  actionForm.action = ReviewAction.Pass;
  actionForm.reason = '';
  actionDialogVisible.value = true;
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
      <el-table-column prop="projectCategoryName" label="项目类别" />
      <el-table-column prop="departmentName" label="所属部门" />
      <el-table-column prop="principalName" label="负责人" />
      <el-table-column prop="currentNode" label="节点" />
      <el-table-column label="操作" width="140">
        <template #default="scope">
          <el-button type="primary" link @click="openAction(scope.row)">审核</el-button>
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
  </div>
</template>
