<script setup lang="ts">
import { reactive } from 'vue';
import { useRouter } from 'vue-router';
import { ElMessage } from 'element-plus';
import { loginApi } from '@/api/auth';
import { useAuthStore } from '@/store/auth';

const router = useRouter();
const auth = useAuthStore();
const form = reactive({ jobNumber: '', password: '' });

const submit = async () => {
  const res = await loginApi(form);
  auth.setToken(res.data.data.accessToken);
  await auth.fetchMe();
  ElMessage.success('登录成功');
  router.push('/declarations');
};
</script>

<template>
  <div style="height:100vh;display:flex;align-items:center;justify-content:center;background:#f0f2f5">
    <el-card style="width:420px">
      <h2 style="margin-top:0">教学质量工程项目申报管理系统</h2>
      <el-form label-position="top">
        <el-form-item label="工号">
          <el-input v-model="form.jobNumber" placeholder="请输入工号" />
        </el-form-item>
        <el-form-item label="密码">
          <el-input v-model="form.password" placeholder="请输入密码" show-password />
        </el-form-item>
        <el-button type="primary" style="width:100%" @click="submit">登录</el-button>
      </el-form>
    </el-card>
  </div>
</template>
