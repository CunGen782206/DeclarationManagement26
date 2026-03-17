import type { DeclarationStatus } from './common';

export interface LoginResponse { accessToken: string; expiresAt: string; }
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
}

export interface TaskItem { id: number; taskName: string; startAt: string; endAt: string; isEnabled: boolean; }
export interface OptionItem { id: number; name: string; }

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
