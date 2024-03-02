## v.1.3.2 2021/10/21

- [调整] Pair 现在会比对Dongle中的耳机SN来检查是否配对
- [调整] ReadDonlgePairedId 现在会读取耳机SN

## v1.3.1 2021/10/18

### 程序

- [新增] 添加CheckDeviceComPortsState()来检查COM状态，以获取颜色ID来辨别

### 测试项目

- [新增] CheckDeviceComPortsState（检查设备COM口状态）

## v1.3.0 2021/10/16

### 测试项目

- [新增] 检查耳机与Dongle条码是否绑定的测试项目

## v1.2.3 2021/10/13

### 程序

- [更新] 适配SDK到v2.5.0-rc
- [新增] 异常信息将保存在文件中
- [回调] COM口资源将由程序自动回收

## v1.2.2  2021/10/9

### 程序

- [修复] 所有的COM会在第一项测试前创建，在最后一项测试后销毁

### 测试项目

- [新增] PowerOff（ByDongle）从Dongle将耳机关机

- [调整] PowerOff改为从耳机下指令将耳机关机

## v1.2.1  2021/9/22

### 程序

- [修复] HeadsetRSSI2440、HeadsetRSSI2480两个测试项补偿值错误的问题
- [修复] 开关屏蔽箱偶尔会失效的Bug，现在指令会**立即写入(Flush)**
- [优化] 依据料号卡控颜色的代码增加了对85X660004、85X660005、85X660006三个料号的支持

## v1.2.0

### 程序

- [调整] 更改Config.txt配置文件，详情见[README.html]()

### 测试项目

- [调整] OpenShieldingBox/CloseDongleSwitch 将不再等待返回值

- ~~[新增] 添加OpenDongleSwitch、CloseDongleSwitch、OpenHeadsetSwitch、CloseHeadsetSwitch来控制继电器~~

### 文档 

- [调整] README.html 配置文件章节更新了部分配置项

## v.1.1.1 2021/9/15 

### 程序

- [修复] 控制屏蔽箱失效的Bug，现在改为在机型Dll代码中实现
- [调整] 开关屏蔽箱的指令改为在[Config.txt]()的[OpenBoxCmd]()和[CloseBoxCmd]()中配置

### 文档

- [更新] README.html文件

## v1.1 2021/9/14

### 程序

- [修复] SN读值错误

## v1.0.0 2021/9/8

### 程序



- [优化] 如果不手动配置设备的COM口，**将自动获取** （如果有多个相同的设备（如两个Donlge在测试），请手动设置）
- [调整] 配置文件改为同目录下的 [Config.txt]() 而非[~~message.txt~~]()

### 测试项目

- [新增] 读DonlgeTestFlag、读耳机TestFlag
- [调整] 读取SN的测试项目，如果读出的SN为默认的SN，则False

### 文档

- [新增] README.md自述文件

