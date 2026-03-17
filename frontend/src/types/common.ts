export interface ApiResponse<T> {
  success: boolean;
  message: string;
  data: T;
}

export interface PagedResult<T> {
  pageIndex: number;
  pageSize: number;
  totalCount: number;
  items: T[];
}

export enum DeclarationStatus {
  Draft = 1,
  PendingPreReview = 2,
  PreReviewRejected = 3,
  PreReviewNotPassed = 4,
  PendingInitialReview = 5,
  InitialReviewRejected = 6,
  InitialReviewNotPassed = 7,
  InitialReviewApproved = 8
}

export enum ReviewStage {
  PreReview = 1,
  InitialReview = 2
}

export enum ReviewAction {
  Pass = 1,
  NotPass = 2,
  Reject = 3
}
