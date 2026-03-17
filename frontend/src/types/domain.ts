import type { DeclarationStatus, ReviewAction, ReviewStage } from './common';

export interface LoginResponse {
  accessToken: string;
  expiresAt: string;
}

export interface CurrentUser {
  userId: number;
  jobNumber: string;
  fullName: string;
  departmentId: number;
  isSuperAdmin: boolean;
}

export interface DeclarationItem {
  id: number;
  projectName: string;
  projectCategoryName: string;
  departmentName: string;
  projectLevel: number;
  awardLevel: number;
  participationType: number;
  principalName: string;
  currentStatus: DeclarationStatus;
  taskName: string;
  submittedAt?: string;
  action: string;
}

export interface DeclarationDetail {
  id: number;
  taskId: number;
  taskName: string;
  applicantName: string;
  principalName: string;
  contactPhone: string;
  departmentId: number;
  departmentName: string;
  projectCategoryId: number;
  projectCategoryName: string;
  projectName: string;
  projectLevel: number;
  awardLevel: number;
  participationType: number;
  approvalDocumentName?: string;
  sealUnitAndDate?: string;
  projectContent?: string;
  projectAchievement?: string;
  currentStatus: DeclarationStatus;
  currentNode: number;
  versionNo: number;
  submittedAt?: string;
}

export interface AttachmentItem {
  id: number;
  originalFileName: string;
  fileSizeBytes: number;
  uploadedAt: string;
}

export interface TaskItem {
  id: number;
  taskName: string;
  startAt: string;
  endAt: string;
  isEnabled: boolean;
}

export interface UserItem {
  id: number;
  jobNumber: string;
  fullName: string;
  departmentId: number;
  departmentName: string;
  isEnabled: boolean;
  isSuperAdmin: boolean;
  preReviewDepartmentIds: number[];
  initialReviewCategoryIds: number[];
}

export interface PendingReviewItem {
  declarationId: number;
  projectName: string;
  projectCategoryName: string;
  departmentName: string;
  currentStatus: DeclarationStatus;
  currentReviewStage: ReviewStage;
  submittedAt?: string;
}

export interface ReviewRecordItem {
  id: number;
  declarationId: number;
  reviewStage: ReviewStage;
  reviewAction: ReviewAction;
  reason?: string;
  recognizedProjectLevel?: number;
  recognizedAwardLevel?: number;
  recognizedAmount?: number;
  remark?: string;
  reviewedByUserId: number;
  reviewedAt: string;
}

export interface FlowLogItem {
  id: number;
  declarationId: number;
  fromStatus?: DeclarationStatus;
  toStatus: DeclarationStatus;
  actionType: number;
  operatorUserId: number;
  note?: string;
  createdAt: string;
}

export interface StatisticsItem {
  declarationId: number;
  projectName: string;
  projectCategoryName: string;
  departmentName: string;
  applicantName: string;
  projectLevel: number;
  awardLevel: number;
  participationType: number;
  contactPhone: string;
  sealUnitAndDate?: string;
  finalReviewDepartmentName: string;
  statusName: string;
  reviewReason?: string;
  recognizedProjectLevel?: number;
  recognizedAwardLevel?: number;
  recognizedAmount?: number;
  remark?: string;
  status: DeclarationStatus;
  submittedAt?: string;
}
