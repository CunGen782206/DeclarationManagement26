<script setup lang="ts">
import { onMounted, reactive, ref } from 'vue';
import { useRouter } from 'vue-router';
import { getMyDeclarationsApi } from '@/api/declaration';
import StatusTag from '@/components/StatusTag.vue';
import type { DeclarationItem } from '@/types/domain';
import { categoryOptions, declarationStatusOptions, departmentOptions } from '@/utils/constants';

const router = useRouter();
const loading = ref(false);
const rows = ref<DeclarationItem[]>([]);
const total = ref(0);

const query = reactive({
  pageIndex: 1,
  pageSize: 10,
  startDate: '',
  endDate: '',
  departmentIds: [] as number[],
  categoryIds: [] as number[],
  statuses: [] as number[]
});

const loadData = async () => {
  loading.value = true;
  try {
    const res = await getMyDeclarationsApi(query);
    rows.value = res.data.data.items;
    total.value = res.data.data.totalCount;
  } finally {
    loading.value = false;
  }
};

onMounted(loadData);
</script>

<template>
  <div class="page-container">
    <div class="toolbar" style="display: grid; grid-template-columns: repeat(3, minmax(220px, 1fr)); gap: 12px">
      <el-date-picker v-model="query.startDate" type="date" placeholder="开始日期" value-format="YYYY-MM-DD" />
      <el-date-picker v-model="query.endDate" type="date" placeholder="结束日期" value-format="YYYY-MM-DD" />
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
        <el-button type="success" @click="router.push('/declarations/new')">新建申报</el-button>
      </div>
    </div>

    <el-table :data="rows" v-loading="loading" border style="margin-top: 16px">
      <el-table-column prop="submittedAt" label="申请时间" width="180" />
      <el-table-column prop="projectCategoryName" label="项目类别" />
      <el-table-column prop="projectName" label="项目名称" />
      <el-table-column prop="departmentName" label="所属部门" />
      <el-table-column prop="principalName" label="负责人" />
      <el-table-column prop="taskName" label="申报任务" />
      <el-table-column label="状态" width="140">
        <template #default="{ row }">
          <StatusTag :status="row.currentStatus" />
        </template>
      </el-table-column>
      <el-table-column label="操作" width="120">
        <template #default="{ row }">
          <el-button link type="primary" @click="router.push(`/declarations/${row.id}`)">{{ row.action }}</el-button>
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
      @size-change="loadData"
    />
  </div>
</template>
