## 植物物候观测系统
* 该项目主要以 `物候数据可视化` 为目标，对全国多个观测站点的植物物候数据进行可视化分析。同时还设计了用户植物物候图片上传功能，以作后期分析使用。
#### 项目涉及技术
* `HTML` `CSS` `JS` `jQuery` `Bootstrap` `Echart` `baidu Map API` `ASP.NET Core MVC` `C#` `Postgres`

##### 物候数据的可视化
1. 根据用户上传图片的坐标信息将图片展示在地图上，并添加聚类显示效果、添加点击事件
<div align=center><img width=750px  src="https://github.com/NorthwesternDirector/plantCamera-Project/blob/master/captures/captures%20(5).png"></div>
<div align=center><img width=750px  src="https://github.com/NorthwesternDirector/plantCamera-Project/blob/master/captures/captures%20(2).png"></div>
<div align=center><img width=750px  src="https://github.com/NorthwesternDirector/plantCamera-Project/blob/master/captures/captures%20(7).png"></div><br>

2. 利用`Echart`与`百度地图`将全国站点全部显示在地图上，选择指定年份与植物种类后便可查看其在全国范围内，在不同经纬度的条件下，植物物候的变化情况。
<div align=center><img width=750px  src="https://github.com/NorthwesternDirector/plantCamera-Project/blob/master/captures/captures%20(8).png"></div>
<div align=center><img width=750px  src="https://github.com/NorthwesternDirector/plantCamera-Project/blob/master/captures/captures%20(1).png"></div>
<div align=center><img width=750px  src="https://github.com/NorthwesternDirector/plantCamera-Project/blob/master/captures/captures%20(4).png"></div><br>

3. 利用`Echart`将从数据接口获得的历年植物物候数据以表格的形式可视化
<div align=center><img width=750px  src="https://github.com/NorthwesternDirector/plantCamera-Project/blob/master/captures/captures%20(6).png"></div><br>
