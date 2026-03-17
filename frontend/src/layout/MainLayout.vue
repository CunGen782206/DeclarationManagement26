<script setup lang="ts">
import { computed } from 'vue';
import { useRouter, useRoute } from 'vue-router';
import { useAuthStore } from '@/store/auth';

const router = useRouter();
const route = useRoute();
const auth = useAuthStore();

const menuItems = computed(() => {
  const base = [
    { index: '/declarations', label: '申报列表' },
    { index: '/reviews', label: '审核页' }
  ];
  if (auth.isSuperAdmin) {
    base.push({ index: '/statistics', label: '统计页' }, { index: '/users', label: '用户管理页' });
  }
  return base;
});

const onSelect = (path: string) => router.push(path);
const logout = () => {
  auth.clearAuth();
  router.push('/login');
};
</script>

<template>
  <el-container style="height: 100vh">
    <el-aside width="220px" style="background: #001529; color: #fff;">
      <div style="padding: 16px; font-size: 16px; font-weight: 600">项目申报系统</div>
      <el-menu :default-active="route.path" background-color="#001529" text-color="#fff" active-text-color="#ffd04b" @select="onSelect">
        <el-menu-item v-for="item in menuItems" :key="item.index" :index="item.index">{{ item.label }}</el-menu-item>
      </el-menu>
    </el-aside>
    <el-container>
      <el-header style="display: flex; justify-content: flex-end; align-items: center; gap: 12px; background: #fff; border-bottom: 1px solid #ebeef5;">
        <el-avatar>用</el-avatar>
        <span>{{ auth.user?.fullName }}</span>
        <el-button link type="danger" @click="logout">退出登录</el-button>
      </el-header>
      <el-main>
        <router-view />
      </el-main>
    </el-container>
  </el-container>
</template>
