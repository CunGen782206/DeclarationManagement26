<script setup lang="ts">
import { reactive, ref } from 'vue';
import { ElMessage } from 'element-plus';
import { exportArchiveApi, exportExcelApi, queryStatisticsApi } from '@/api/statistics';
import { listTasksApi } from '@/api/task';

const tasks = ref<any[]>([]);
const rows = ref<any[]>([]);
const query = reactive({ taskId: undefined as number | undefined, startDate: '', endDate: '' });

const downloadBlob = (blob: Blob, filename: string) => {
  const url = window.URL.createObjectURL(blob);
  const link = document.createElement('a');
  link.href = url;
  link.download = filename;
  link.click();
  window.URL.revokeObjectURL(url);
};

const load = async () => {
  const res = await queryStatisticsApi(query as unknown as Record<string, unknown>);
  rows.value = res.data.data;
};

const exportExcel = async () => {
  const res = await exportExcelApi(query as unknown as Record<string, unknown>);
  downloadBlob(res.data, '统计导出.xlsx');
  ElMessage.success('Excel导出成功');
};

const exportArchive = async () => {
  const res = await exportArchiveApi(query as unknown as Record<string, unknown>);
  downloadBlob(res.data, '归档.zip');
  ElMessage.success('归档导出成功');
};

listTasksApi().then((res) => (tasks.value = res.data.data));
</script>

<template>
  <div class="page-container">
    <div class="toolbar">
      <el-select v-model="query.taskId" placeholder="选择申报任务" style="width: 260px">
        <el-option v-for="t in tasks" :key="t.id" :label="t.taskName" :value="t.id" />
      </el-select>
      <el-button type="primary" @click="load">查询</el-button>
      <el-button @click="exportExcel">下载Excel</el-button>
      <el-button type="success" @click="exportArchive">归档下载</el-button>
    </div>
    <el-table :data="rows" border>
      <el-table-column prop="submittedAt" label="申请时间" width="180" />
      <el-table-column prop="departmentName" label="部门" />
      <el-table-column prop="projectName" label="项目名称" />
      <el-table-column prop="projectCategoryName" label="项目类别" />
      <el-table-column prop="statusName" label="状态" />
    </el-table>
  </div>
</template>
