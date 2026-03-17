export const departmentOptions = [
  '发展规划处','港航工程学院','公用事业和路桥工程学院','轨道交通学院','基础教育部','教务处','经济管理学院','科研处','马克思主义学院','汽车工程学院','人文艺术学院','智慧交通学院','组织人事处'
].map((name, index) => ({ id: index + 1, name }));

export const categoryOptions = ['专业建设类','课程建设类','师资建设类','教学竞赛类','教材建设类','教学成果类']
  .map((name, index) => ({ id: index + 1, name }));

export const projectLevelOptions = [
  { value: 1, label: '国家级' },
  { value: 2, label: '市级' },
  { value: 3, label: '行业/教指委级' },
  { value: 4, label: '校级' }
];

export const awardLevelOptions = [
  { value: 1, label: '一等第' },
  { value: 2, label: '二等第' },
  { value: 3, label: '三等第' },
  { value: 4, label: '四等第' },
  { value: 5, label: '无' }
];

export const participationTypeOptions = [
  { value: 1, label: '个人' },
  { value: 2, label: '团队' }
];
