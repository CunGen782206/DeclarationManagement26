<script setup lang="ts">
import { onMounted, reactive, ref } from 'vue';
import { ElMessage, ElMessageBox } from 'element-plus';
import { createUserApi, deleteUserApi, listUsersApi, resetPasswordApi, updateUserApi } from '@/api/user';
import type { UserItem } from '@/types/domain';
import { categoryOptions, departmentOptions } from '@/utils/constants';

const rows = ref<UserItem[]>([]);
const dialogVisible = ref(false);
const editingId = ref<number | null>(null);

const query = reactive({
  jobNumber: '',
  fullName: '',
  departmentIds: [] as number[]
});

const form = reactive({
  jobNumber: '',
  fullName: '',
  departmentId: undefined as number | undefined,
  isEnabled: true,
  isSuperAdmin: false,
  preReviewDepartmentIds: [] as number[],
  initialReviewCategoryIds: [] as number[]
});

const loadData = async () => {
  const res = await listUsersApi(query);
  rows.value = res.data.data;
};

const openCreate = () => {
  editingId.value = null;
  Object.assign(form, {
    jobNumber: '',
    fullName: '',
    departmentId: undefined,
    isEnabled: true,
    isSuperAdmin: false,
    preReviewDepartmentIds: [],
    initialReviewCategoryIds: []
  });
  dialogVisible.value = true;
};

const openEdit = (row: UserItem) => {
  editingId.value = row.id;
  Object.assign(form, row);
  dialogVisible.value = true;
};

const submit = async () => {
  if (editingId.value) {
    await updateUserApi(editingId.value, form);
  } else {
    await createUserApi(form);
  }

  ElMessage.success('保存成功');
  dialogVisible.value = false;
  await loadData();
};

const remove = async (id: number) => {
  await ElMessageBox.confirm('确认删除该用户吗？', '提示');
  await deleteUserApi(id);
  ElMessage.success('删除成功');
  await loadData();
};

const reset = async (id: number) => {
  await resetPasswordApi(id);
  ElMessage.success('已重置为 111111');
};

onMounted(loadData);
</script>

<template>
  <div class="page-container">
    <div class="toolbar" style="display: grid; grid-template-columns: repeat(4, minmax(200px, 1fr)); gap: 12px">
      <el-input v-model="query.jobNumber" placeholder="工号" />
      <el-input v-model="query.fullName" placeholder="姓名" />
      <el-select v-model="query.departmentIds" multiple collapse-tags placeholder="所属部门">
        <el-option v-for="item in departmentOptions" :key="item.id" :label="item.name" :value="item.id" />
      </el-select>
      <div style="display: flex; gap: 8px">
        <el-button type="primary" @click="loadData">查询</el-button>
        <el-button type="success" @click="openCreate">新增用户</el-button>
      </div>
    </div>

    <el-table :data="rows" border style="margin-top: 16px">
      <el-table-column prop="jobNumber" label="工号" />
      <el-table-column prop="fullName" label="姓名" />
      <el-table-column prop="departmentName" label="所属部门" />
      <el-table-column label="超级管理员" width="120">
        <template #default="{ row }">
          <el-tag :type="row.isSuperAdmin ? 'danger' : 'info'">{{ row.isSuperAdmin ? '是' : '否' }}</el-tag>
        </template>
      </el-table-column>
      <el-table-column label="启用状态" width="120">
        <template #default="{ row }">
          <el-tag :type="row.isEnabled ? 'success' : 'info'">{{ row.isEnabled ? '启用' : '停用' }}</el-tag>
        </template>
      </el-table-column>
      <el-table-column label="操作" width="240">
        <template #default="{ row }">
          <el-button link type="primary" @click="openEdit(row)">编辑</el-button>
          <el-button link type="danger" @click="remove(row.id)">删除</el-button>
          <el-button link @click="reset(row.id)">重置密码</el-button>
        </template>
      </el-table-column>
    </el-table>

    <el-dialog v-model="dialogVisible" title="用户信息" width="720px">
      <el-form label-width="120px">
        <el-form-item label="工号">
          <el-input v-model="form.jobNumber" :disabled="editingId !== null" />
        </el-form-item>
        <el-form-item label="姓名">
          <el-input v-model="form.fullName" />
        </el-form-item>
        <el-form-item label="所属部门">
          <el-select v-model="form.departmentId">
            <el-option v-for="item in departmentOptions" :key="item.id" :value="item.id" :label="item.name" />
          </el-select>
        </el-form-item>
        <el-form-item label="超级管理员">
          <el-switch v-model="form.isSuperAdmin" />
        </el-form-item>
        <el-form-item v-if="editingId !== null" label="启用状态">
          <el-switch v-model="form.isEnabled" />
        </el-form-item>
        <el-form-item label="部门预审权限">
          <el-select v-model="form.preReviewDepartmentIds" multiple>
            <el-option v-for="item in departmentOptions" :key="item.id" :value="item.id" :label="item.name" />
          </el-select>
        </el-form-item>
        <el-form-item label="初审权限">
          <el-select v-model="form.initialReviewCategoryIds" multiple>
            <el-option v-for="item in categoryOptions" :key="item.id" :value="item.id" :label="item.name" />
          </el-select>
        </el-form-item>
      </el-form>
      <template #footer>
        <el-button @click="dialogVisible = false">取消</el-button>
        <el-button type="primary" @click="submit">保存</el-button>
      </template>
    </el-dialog>
  </div>
</template>
