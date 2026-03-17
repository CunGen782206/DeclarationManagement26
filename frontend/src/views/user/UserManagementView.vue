<script setup lang="ts">
import { onMounted, reactive, ref } from 'vue';
import { ElMessage, ElMessageBox } from 'element-plus';
import { createUserApi, deleteUserApi, listUsersApi, resetPasswordApi, updateUserApi } from '@/api/user';
import { categoryOptions, departmentOptions } from '@/utils/constants';

const rows = ref<any[]>([]);
const dialogVisible = ref(false);
const editingId = ref<number | null>(null);
const form = reactive({ jobNumber: '', fullName: '', departmentId: undefined as number | undefined, isEnabled: true, isSuperAdmin: false, preReviewDepartmentIds: [] as number[], initialReviewCategoryIds: [] as number[] });

const loadData = async () => {
  const res = await listUsersApi();
  rows.value = res.data.data;
};

const openCreate = () => {
  editingId.value = null;
  Object.assign(form, { jobNumber: '', fullName: '', departmentId: undefined, isEnabled: true, isSuperAdmin: false, preReviewDepartmentIds: [], initialReviewCategoryIds: [] });
  dialogVisible.value = true;
};

const openEdit = (row: any) => {
  editingId.value = row.id;
  Object.assign(form, row);
  dialogVisible.value = true;
};

const submit = async () => {
  if (editingId.value) {
    await updateUserApi(editingId.value, form as unknown as Record<string, unknown>);
  } else {
    await createUserApi(form as unknown as Record<string, unknown>);
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
  ElMessage.success('已重置为111111');
};

onMounted(loadData);
</script>

<template>
  <div class="page-container">
    <div class="toolbar"><el-button type="primary" @click="openCreate">新增用户</el-button></div>
    <el-table :data="rows" border>
      <el-table-column prop="jobNumber" label="工号" />
      <el-table-column prop="fullName" label="姓名" />
      <el-table-column prop="departmentName" label="所属部门" />
      <el-table-column label="操作" width="240">
        <template #default="scope">
          <el-button link type="primary" @click="openEdit(scope.row)">编辑</el-button>
          <el-button link type="danger" @click="remove(scope.row.id)">删除</el-button>
          <el-button link @click="reset(scope.row.id)">重置密码</el-button>
        </template>
      </el-table-column>
    </el-table>

    <el-dialog v-model="dialogVisible" title="用户信息" width="700px">
      <el-form label-width="120px">
        <el-form-item label="工号"><el-input v-model="form.jobNumber" :disabled="editingId !== null" /></el-form-item>
        <el-form-item label="姓名"><el-input v-model="form.fullName" /></el-form-item>
        <el-form-item label="所属部门"><el-select v-model="form.departmentId"><el-option v-for="i in departmentOptions" :key="i.id" :value="i.id" :label="i.name" /></el-select></el-form-item>
        <el-form-item label="部门预审权限"><el-select v-model="form.preReviewDepartmentIds" multiple><el-option v-for="i in departmentOptions" :key="i.id" :value="i.id" :label="i.name" /></el-select></el-form-item>
        <el-form-item label="初审权限"><el-select v-model="form.initialReviewCategoryIds" multiple><el-option v-for="i in categoryOptions" :key="i.id" :value="i.id" :label="i.name" /></el-select></el-form-item>
      </el-form>
      <template #footer><el-button @click="dialogVisible=false">取消</el-button><el-button type="primary" @click="submit">保存</el-button></template>
    </el-dialog>
  </div>
</template>
