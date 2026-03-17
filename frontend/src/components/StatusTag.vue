<script setup lang="ts">
import { computed } from 'vue';
import { DeclarationStatus } from '@/types/common';

const props = defineProps<{ status: DeclarationStatus }>();

const map = {
  [DeclarationStatus.Draft]: { text: '草稿', type: 'info' },
  [DeclarationStatus.PendingPreReview]: { text: '待预审核', type: 'warning' },
  [DeclarationStatus.PreReviewRejected]: { text: '预审核驳回', type: 'danger' },
  [DeclarationStatus.PreReviewNotPassed]: { text: '预审核不通过', type: 'danger' },
  [DeclarationStatus.PendingInitialReview]: { text: '待初审', type: 'warning' },
  [DeclarationStatus.InitialReviewRejected]: { text: '初审驳回', type: 'danger' },
  [DeclarationStatus.InitialReviewNotPassed]: { text: '初审不通过', type: 'danger' },
  [DeclarationStatus.InitialReviewApproved]: { text: '初审通过', type: 'success' }
} as const;

const statusData = computed(() => map[props.status] ?? { text: '未知', type: 'info' });
</script>

<template>
  <el-tag :type="statusData.type">{{ statusData.text }}</el-tag>
</template>
