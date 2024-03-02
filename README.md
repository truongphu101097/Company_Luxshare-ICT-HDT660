## 目录结构

在此章节了解每个文件的作用

> **提示**
>
> 下面显示的目录指 {测试程序所在目录}\TestItem\HDT660，并只解释一些不可缺少的文件

```bash
├── HDT660.dll 				# 机型dll
├── MESDLL.dll 				# MES接口的dll，被机型dll引用
├── SWDevelopKit.dll 		# SDK，被机型dll引用
├── Config.txt 				# 配置文件，一些参数会影响到机型dll的行为或功能
├── CHANGELOG.html 			# 更新日志，必读
├── README.html 			# 自述文件，必读
└── {站别}.txt			   # 一些站别测试项目文件，在此不做阐述
```

## 配置文件

在此章节掌握如何配置机型Dll

> **提示**
>
> - 此配置指机型Dll同目录下的 [Config.txt]()
>
> - 带`（废弃的）`的配置项将不会再Dll中使用

```ini
# 以工单(WO)或料号(PN)来检查颜色
CheckColorIDBy=WO
# 耳机COM口 不设置则自动获取
HeadsetComPort=
# DongleCOM口 不设置则自动获取
DongleComPort=
# 继电器COM口（废弃的）
SwichComPort=Com2
# 屏蔽箱COM口
ShieldingBoxPort=Com1
# RF补偿值
Dongle2402=0
Dongle2440=0
Dongle2480=0
Headset2402=0
Headset2440=0
Headset2480=0

# 控制继电器的指令（废弃的）
OpenDongleSwitchCmd=open1
CloseDongleSwitchCmd=close1
OpenHeadsetSwitchCmd=open2
CloseHeadsetSwitchCmd=close2

# 控制屏蔽箱开关的指令
OpenBoxCmd=open
CloseBoxCmd=close
```

## 其他事项

Todo