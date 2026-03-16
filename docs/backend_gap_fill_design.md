# 后端补齐设计（按PRD）

## 目标
按 PRD 补齐后端核心功能：
- Auth（JWT 登录、当前用户、修改密码）
- Users（增删改查、重置密码、预审/初审权限关联维护）
- Tasks（新建任务、时间窗更新、启停）
- Declarations（草稿/提交、驳回后修改重提、我的列表、详情）
- Reviews（待审列表、预审/初审动作、审核记录累加、流程日志）
- Attachments（上传、下载）
- Statistics（筛选列表、Excel导出、归档zip，归档仅初审通过）

## 状态机
- Draft -> PendingPreReview（提交）
- PendingPreReview -> PendingInitialReview（预审通过）
- PendingPreReview -> PreReviewNotPassed（预审不通过）
- PendingPreReview -> PreReviewRejected（预审驳回）
- PreReviewRejected/InitialReviewRejected -> PendingPreReview（重提）
- PendingInitialReview -> InitialReviewApproved（初审通过）
- PendingInitialReview -> InitialReviewNotPassed（初审不通过）
- PendingInitialReview -> InitialReviewRejected（初审驳回）

## 关键约束
- 审核动作必须同时写 `DeclarationReviewRecord` + `DeclarationFlowLog`
- 权限为多对多关联表（不拼接字符串）
- 统计导出按筛选结果
- 归档仅包含初审通过
