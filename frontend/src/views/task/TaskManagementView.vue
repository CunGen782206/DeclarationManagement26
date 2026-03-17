<script setup lang="ts">
import { onMounted, reactive, ref } from 'vue';
import { ElMessage } from 'element-plus';
import { createTaskApi, listTasksApi, updateTaskStatusApi, updateTaskWindowApi } from '@/api/task';

const rows = ref<any[]>([]);
const createDialogVisible = ref(false);
const windowDialogVisible = ref(false);
const createForm = reactive({ taskName: '', startAt: '', endAt: '' });
const windowForm = reactive({ id: 0, startAt: '', endAt: '' });

const loadData = async () => {
  const res = await listTasksApi();
  rows.value = res.data.data;
};

const openWindowDialog = (row: any) => {
  windowForm.id = row.id;
  windowForm.startAt = row.startAt;
  windowForm.endAt = row.endAt;
  windowDialogVisible.value = true;
};

const createTask = async () => {
  await createTaskApi(createForm as unknown as Record<string, unknown>);
  ElMessage.success('任务创建成功');
  createDialogVisible.value = false;
  await loadData();
};

const updateWindow = async () => {
  await updateTaskWindowApi(windowForm.id, {
    startAt: windowForm.startAt,
    endAt: windowForm.endAt
  });
  ElMessage.success('时间窗修改成功');
  windowDialogVisible.value = false;
  await loadData();
};

const toggleStatus = async (row: any) => {
  await updateTaskStatusApi(row.id, { isEnabled: !row.isEnabled });
  ElMessage.success('状态修改成功');
  await loadData();
};

onMounted(loadData);
</script>

<template>
  <div class="page-container">
    <div class="toolbar">
      <el-button type="primary" @click="createDialogVisible = true">新建申报任务</el-button>
    </div>

    <el-table :data="rows" border>
      <el-table-column prop="taskName" label="任务名称" />
      <el-table-column prop="startAt" label="开始时间" width="200" />
      <el-table-column prop="endAt" label="结束时间" width="200" />
      <el-table-column label="启用状态" width="120">
        <template #default="scope">
          <el-tag :type="scope.row.isEnabled ? 'success' : 'info'">{{ scope.row.isEnabled ? '启用' : '停用' }}</el-tag>
        </template>
      </el-table-column>
      <el-table-column label="操作" width="220">
        <template #default="scope">
          <el-button type="primary" link @click="openWindowDialog(scope.row)">修改时间窗</el-button>
          <el-button type="warning" link @click="toggleStatus(scope.row)">{{ scope.row.isEnabled ? '停用' : '启用' }}</el-button>
        </template>
      </el-table-column>
    </el-table>

    <el-dialog v-model="createDialogVisible" title="新建申报任务" width="520px">
      <el-form label-width="90px">
        <el-form-item label="任务名称"><el-input v-model="createForm.taskName" /></el-form-item>
        <el-form-item label="开始时间"><el-date-picker v-model="createForm.startAt" type="datetime" /></el-form-item>
        <el-form-item label="结束时间"><el-date-picker v-model="createForm.endAt" type="datetime" /></el-form-item>
      </el-form>
      <template #footer>
        <el-button @click="createDialogVisible = false">取消</el-button>
        <el-button type="primary" @click="createTask">创建</el-button>
      </template>
    </el-dialog>

    <el-dialog v-model="windowDialogVisible" title="修改任务时间窗" width="520px">
      <el-form label-width="90px">
        <el-form-item label="开始时间"><el-date-picker v-model="windowForm.startAt" type="datetime" /></el-form-item>
        <el-form-item label="结束时间"><el-date-picker v-model="windowForm.endAt" type="datetime" /></el-form-item>
      </el-form>
      <template #footer>
        <el-button @click="windowDialogVisible = false">取消</el-button>
        <el-button type="primary" @click="updateWindow">保存</el-button>
      </template>
    </el-dialog>
  </div>
</template>
