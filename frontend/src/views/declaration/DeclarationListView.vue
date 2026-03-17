<script setup lang="ts">
import { onMounted, reactive, ref } from 'vue';
import { useRouter } from 'vue-router';
import { getMyDeclarationsApi } from '@/api/declaration';
import StatusTag from '@/components/StatusTag.vue';
import type { DeclarationItem } from '@/types/domain';

const router = useRouter();
const loading = ref(false);
const rows = ref<DeclarationItem[]>([]);
const total = ref(0);
const query = reactive({ pageIndex: 1, pageSize: 10, startDate: '', endDate: '' });

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
    <div class="toolbar">
      <el-date-picker v-model="query.startDate" type="date" placeholder="开始日期" />
      <el-date-picker v-model="query.endDate" type="date" placeholder="结束日期" />
      <el-button type="primary" @click="loadData">查询</el-button>
      <el-button type="success" @click="router.push('/declarations/new')">新建申报</el-button>
    </div>
    <el-table :data="rows" v-loading="loading" border>
      <el-table-column prop="submittedAt" label="申请时间" width="180" />
      <el-table-column prop="projectCategoryName" label="项目类别" />
      <el-table-column prop="projectName" label="项目名称" />
      <el-table-column prop="departmentName" label="所属部门" />
      <el-table-column prop="principalName" label="负责人" />
      <el-table-column label="状态" width="140">
        <template #default="scope"><StatusTag :status="scope.row.currentStatus" /></template>
      </el-table-column>
      <el-table-column prop="taskName" label="申报任务" />
      <el-table-column label="操作" width="120">
        <template #default="scope">
          <el-button link type="primary" @click="router.push(`/declarations/${scope.row.id}`)">查看/编辑</el-button>
        </template>
      </el-table-column>
    </el-table>
    <el-pagination
      style="margin-top:16px"
      v-model:current-page="query.pageIndex"
      v-model:page-size="query.pageSize"
      :total="total"
      layout="total, prev, pager, next, jumper"
      @current-change="loadData"
      @size-change="loadData"
    />
  </div>
</template>
