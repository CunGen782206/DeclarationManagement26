<script setup lang="ts">
import { computed, onMounted, reactive, ref } from 'vue';
import { useRoute, useRouter } from 'vue-router';
import { ElMessage } from 'element-plus';
import { createDeclarationApi, getDeclarationDetailApi, resubmitDeclarationApi, submitDeclarationApi, updateDeclarationApi } from '@/api/declaration';
import { listTasksApi } from '@/api/task';
import { awardLevelOptions, categoryOptions, departmentOptions, participationTypeOptions, projectLevelOptions } from '@/utils/constants';

const route = useRoute();
const router = useRouter();
const declarationId = computed(() => Number(route.params.id || 0));
const tasks = ref<{ id: number; taskName: string }[]>([]);

const form = reactive({
  taskId: undefined as number | undefined,
  principalName: '',
  contactPhone: '',
  departmentId: undefined as number | undefined,
  projectName: '',
  projectCategoryId: undefined as number | undefined,
  projectLevel: 1,
  awardLevel: 1,
  participationType: 1,
  approvalDocumentName: '',
  sealUnitAndDate: '',
  projectContent: '',
  projectAchievement: ''
});

const save = async () => {
  if (!declarationId.value) {
    const res = await createDeclarationApi(form as unknown as Record<string, unknown>);
    ElMessage.success('创建成功');
    router.replace(`/declarations/${res.data.data}`);
    return;
  }
  await updateDeclarationApi(declarationId.value, form as unknown as Record<string, unknown>);
  ElMessage.success('保存成功');
};

const submit = async () => {
  if (!declarationId.value) return;
  await submitDeclarationApi(declarationId.value);
  ElMessage.success('提交成功');
};

const resubmit = async () => {
  if (!declarationId.value) return;
  await resubmitDeclarationApi(declarationId.value);
  ElMessage.success('重提成功');
};

onMounted(async () => {
  const taskRes = await listTasksApi();
  tasks.value = taskRes.data.data;
  if (declarationId.value) {
    const res = await getDeclarationDetailApi(declarationId.value);
    Object.assign(form, res.data.data);
  }
});
</script>

<template>
  <div class="page-container">
    <el-card>
      <template #header>申报表单</template>
      <el-form label-width="120px">
        <el-row :gutter="16">
          <el-col :span="12"><el-form-item label="申报任务"><el-select v-model="form.taskId"><el-option v-for="t in tasks" :key="t.id" :value="t.id" :label="t.taskName" /></el-select></el-form-item></el-col>
          <el-col :span="12"><el-form-item label="负责人"><el-input v-model="form.principalName" /></el-form-item></el-col>
          <el-col :span="12"><el-form-item label="联系方式"><el-input v-model="form.contactPhone" /></el-form-item></el-col>
          <el-col :span="12"><el-form-item label="所属部门"><el-select v-model="form.departmentId"><el-option v-for="i in departmentOptions" :key="i.id" :value="i.id" :label="i.name" /></el-select></el-form-item></el-col>
          <el-col :span="12"><el-form-item label="项目名称"><el-input v-model="form.projectName" /></el-form-item></el-col>
          <el-col :span="12"><el-form-item label="项目类别"><el-select v-model="form.projectCategoryId"><el-option v-for="i in categoryOptions" :key="i.id" :value="i.id" :label="i.name" /></el-select></el-form-item></el-col>
          <el-col :span="12"><el-form-item label="项目等级"><el-select v-model="form.projectLevel"><el-option v-for="i in projectLevelOptions" :key="i.value" :value="i.value" :label="i.label" /></el-select></el-form-item></el-col>
          <el-col :span="12"><el-form-item label="奖项级别"><el-select v-model="form.awardLevel"><el-option v-for="i in awardLevelOptions" :key="i.value" :value="i.value" :label="i.label" /></el-select></el-form-item></el-col>
          <el-col :span="12"><el-form-item label="参与形式"><el-select v-model="form.participationType"><el-option v-for="i in participationTypeOptions" :key="i.value" :value="i.value" :label="i.label" /></el-select></el-form-item></el-col>
          <el-col :span="12"><el-form-item label="认定批文名称"><el-input v-model="form.approvalDocumentName" /></el-form-item></el-col>
          <el-col :span="12"><el-form-item label="盖章单位及时间"><el-input v-model="form.sealUnitAndDate" /></el-form-item></el-col>
          <el-col :span="24"><el-form-item label="项目内容"><el-input type="textarea" v-model="form.projectContent" /></el-form-item></el-col>
          <el-col :span="24"><el-form-item label="项目成果"><el-input type="textarea" v-model="form.projectAchievement" /></el-form-item></el-col>
        </el-row>
      </el-form>
      <div class="toolbar">
        <el-button type="primary" @click="save">保存</el-button>
        <el-button type="success" @click="submit" :disabled="!declarationId">提交</el-button>
        <el-button @click="resubmit" :disabled="!declarationId">驳回后重提</el-button>
      </div>
    </el-card>
  </div>
</template>
