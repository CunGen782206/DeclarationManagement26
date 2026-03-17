<script setup lang="ts">
import { onMounted, reactive, ref } from 'vue';
import { ElMessage } from 'element-plus';
import { exportArchiveApi, exportExcelApi, queryStatisticsApi } from '@/api/statistics';
import { listTasksApi } from '@/api/task';
import type { StatisticsItem, TaskItem } from '@/types/domain';
import { categoryOptions, declarationStatusOptions, departmentOptions } from '@/utils/constants';

const tasks = ref<TaskItem[]>([]);
const rows = ref<StatisticsItem[]>([]);

const query = reactive({
  taskId: undefined as number | undefined,
  startDate: '',
  endDate: '',
  departmentIds: [] as number[],
  categoryIds: [] as number[],
  statuses: [] as number[]
});

const downloadBlob = (blob: Blob, filename: string) => {
  const url = window.URL.createObjectURL(blob);
  const link = document.createElement('a');
  link.href = url;
  link.download = filename;
  link.click();
  window.URL.revokeObjectURL(url);
};

const load = async () => {
  const res = await queryStatisticsApi(query);
  rows.value = res.data.data;
};

const exportExcel = async () => {
  const res = await exportExcelApi(query);
  downloadBlob(res.data, '统计导出.xlsx');
  ElMessage.success('Excel 导出成功');
};

const exportArchive = async () => {
  const res = await exportArchiveApi(query);
  downloadBlob(res.data, '归档导出.zip');
  ElMessage.success('归档导出成功');
};

onMounted(async () => {
  const res = await listTasksApi();
  tasks.value = res.data.data;
});
</script>

<template>
  <div class="page-container">
    <div class="toolbar" style="display: grid; grid-template-columns: repeat(3, minmax(220px, 1fr)); gap: 12px">
      <el-select v-model="query.taskId" clearable placeholder="选择申报任务">
        <el-option v-for="task in tasks" :key="task.id" :label="task.taskName" :value="task.id" />
      </el-select>
      <el-date-picker v-model="query.startDate" type="date" placeholder="开始日期" value-format="YYYY-MM-DD" />
      <el-date-picker v-model="query.endDate" type="date" placeholder="结束日期" value-format="YYYY-MM-DD" />
      <el-select v-model="query.departmentIds" multiple collapse-tags placeholder="所属部门">
        <el-option v-for="item in departmentOptions" :key="item.id" :label="item.name" :value="item.id" />
      </el-select>
      <el-select v-model="query.categoryIds" multiple collapse-tags placeholder="项目类别">
        <el-option v-for="item in categoryOptions" :key="item.id" :label="item.name" :value="item.id" />
      </el-select>
      <el-select v-model="query.statuses" multiple collapse-tags placeholder="处理状态">
        <el-option v-for="item in declarationStatusOptions" :key="item.value" :label="item.label" :value="item.value" />
      </el-select>
    </div>

    <div class="toolbar" style="margin-top: 12px">
      <el-button type="primary" @click="load">查询</el-button>
      <el-button @click="exportExcel">下载 Excel</el-button>
      <el-button type="success" @click="exportArchive">归档下载</el-button>
    </div>

    <el-table :data="rows" border style="margin-top: 16px">
      <el-table-column prop="submittedAt" label="申请时间" width="180" />
      <el-table-column prop="departmentName" label="部门" />
      <el-table-column prop="projectName" label="项目名称" />
      <el-table-column prop="projectCategoryName" label="项目类别" />
      <el-table-column prop="applicantName" label="负责人" />
      <el-table-column prop="statusName" label="状态" />
      <el-table-column prop="finalReviewDepartmentName" label="审核部门" />
      <el-table-column prop="reviewReason" label="原因" />
    </el-table>
  </div>
</template>
