# ExamApi

ExamAPIdemo

## 调用方式

默认地址 http://localhost:5192 ，启动后会自动打开 swagger。

### 生成考生并排序

GET /api/candidates/generate?count=25

count 默认 25，小于 20 会返回 400。

返回里同时给了原始列表和排序后的列表，方便看结果：

```
{
  "original":  [ {"id":0,"name":"L0"}, ... ],
  "reordered": [ {"id":0,"name":"L0"}, {"id":24,"name":"L24"}, ... ]
}
```

curl：

```
curl "http://localhost:5192/api/candidates/generate?count=22"
```

### 自己传列表去排序

POST /api/candidates/reorder

body 是考生数组：

```
[
  {"id":1,"name":"L0"},
  {"id":2,"name":"L1"},
  {"id":3,"name":"L2"},
  {"id":4,"name":"L3"}
]
```

返回：

```
[
  {"id":1,"name":"L0"},
  {"id":4,"name":"L3"},
  {"id":2,"name":"L1"},
  {"id":3,"name":"L2"}
]
```

curl（Windows 下引号要转义）：

```
curl -X POST http://localhost:5192/api/candidates/reorder -H "Content-Type: application/json" -d "[{\"id\":1,\"name\":\"L0\"},{\"id\":2,\"name\":\"L1\"},{\"id\":3,\"name\":\"L2\"},{\"id\":4,\"name\":\"L3\"}]"
```

直接打开 swagger 点 Try it out 最省事。

### 健康检查

GET /health

会检查两个东西：
- 数据库能不能连上（用的 SQLite，本地文件 exam.db，程序起来会自动建，不用管）
- 第三方服务能不能连上（随便找了个 httpbin.org 做示例，断网就会 Unhealthy）

## 详细说明

- 排序用的双指针，一个从头一个从尾往中间走，相遇就停。奇数长度时中间那个只取一次。时间复杂度 O(n)。
- 异常统一走中间件 GlobalExceptionMiddleware，ArgumentException 返回 400，其他返回 500，响应体固定 `{status, message}`。
- 数据库选 SQLite 纯粹是为了零配置，换成别的改下 UseSqlite 和连接字符串就行。
- Swagger 默认开着，根路径 / 会自动跳到 /swagger。
- 单元测试覆盖了偶数、奇数、单元素、空列表、数量校验这几种情况，一共 8 个，`dotnet test` 跑。
