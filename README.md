# AppNameLocalization

Unity3d中 安卓和IOS应用名本地化工具

本工具支持2018.4及以上Unity3d.  Andorid打包 只支持Gradle方式。 不支持Internal.

## 使用方法

1. 导入工具到项目内任意位置。

2. 创建应用名多语言配置

   + 修改配置路径  路径配置在 [AppNamePathUtility](./Editor/Utility/AppNamePathUtility.cs)  可自行修改

   + 创建配置

     `工程Project目录下右键->Create/AppNameConfigs/Android`

     `工程Project目录下右键->Create/AppNameConfigs/IOS`

     ![image](https://tva1.sinaimg.cn/large/e1b1a94bly1h7eiz7imc0j20ft05s0t0.jpg)

     `Normal Config` 默认使用的应用名 如果你某个语言没有设置本地化 那么会使用它

     `Configs`  所有的本地化配置  

     * `Code` 本地化代码  关于本地化代码应该填什么 请自行查阅 andorid ios相关文档。
     * `App Name` 应用名

3. 正常打包即可。 

## 引用

https://github.com/zeyangl/UnityAppNameLocalizationForIOS

[Unity安卓应用名称多语言本地化 - 简书 (jianshu.com)](https://www.jianshu.com/p/f88f55a1c044)

