<script setup lang="ts">
import { computed, reactive, ref } from 'vue';
import { ElMessage } from 'element-plus';
import { useRouter, useRoute } from 'vue-router';
import { changePasswordApi } from '@/api/auth';
import { useAuthStore } from '@/store/auth';

const router = useRouter();
const route = useRoute();
const auth = useAuthStore();

const passwordDialogVisible = ref(false);
const passwordForm = reactive({
  oldPassword: '',
  newPassword: ''
});

const menuItems = computed(() => {
  const items = [
    { index: '/declarations', label: '项目申报管理' },
    { index: '/reviews', label: '审核管理' }
  ];

  if (auth.isSuperAdmin) {
    items.push(
      { index: '/tasks', label: '申报任务管理' },
      { index: '/statistics', label: '统计导出' },
      { index: '/users', label: '用户管理' }
    );
  }

  return items;
});

const onSelect = (path: string) => router.push(path);

const logout = () => {
  auth.clearAuth();
  router.push('/login');
};

const openChangePassword = () => {
  passwordForm.oldPassword = '';
  passwordForm.newPassword = '';
  passwordDialogVisible.value = true;
};

const submitChangePassword = async () => {
  await changePasswordApi(passwordForm);
  ElMessage.success('密码修改成功');
  passwordDialogVisible.value = false;
};
</script>

<template>
  <el-container style="height: 100vh">
    <el-aside width="220px" style="background: #001529; color: #fff">
      <div style="padding: 16px; font-size: 16px; font-weight: 600">教学质量工程申报系统</div>
      <el-menu
        :default-active="route.path"
        background-color="#001529"
        text-color="#fff"
        active-text-color="#ffd04b"
        @select="onSelect"
      >
        <el-menu-item v-for="item in menuItems" :key="item.index" :index="item.index">
          {{ item.label }}
        </el-menu-item>
      </el-menu>
    </el-aside>

    <el-container>
      <el-header style="display: flex; justify-content: flex-end; align-items: center; gap: 12px; background: #fff; border-bottom: 1px solid #ebeef5">
        <el-avatar>{{ auth.user?.fullName?.slice(0, 1) || 'U' }}</el-avatar>
        <span>{{ auth.user?.fullName }}</span>
        <el-button link @click="openChangePassword">修改密码</el-button>
        <el-button link type="danger" @click="logout">退出登录</el-button>
      </el-header>

      <el-main>
        <router-view />
      </el-main>
    </el-container>
  </el-container>

  <el-dialog v-model="passwordDialogVisible" title="修改密码" width="420px">
    <el-form label-width="90px">
      <el-form-item label="原密码">
        <el-input v-model="passwordForm.oldPassword" show-password />
      </el-form-item>
      <el-form-item label="新密码">
        <el-input v-model="passwordForm.newPassword" show-password />
      </el-form-item>
    </el-form>
    <template #footer>
      <el-button @click="passwordDialogVisible = false">取消</el-button>
      <el-button type="primary" @click="submitChangePassword">保存</el-button>
    </template>
  </el-dialog>
</template>
