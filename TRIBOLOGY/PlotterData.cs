using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//引用命名空间
using Microsoft.Research.DynamicDataDisplay.DataSources;


namespace TRIBOLOGY
{
    class PlotterData
    {
        //坐标点数据集合
        internal static PlotPointCollection presPtCollection = new PlotPointCollection(200); //压力坐标点集合
        internal static PlotPointCollection platSpePtCollection = new PlotPointCollection(200); //平台转速坐标点集合
        internal static PlotPointCollection fricForPtCollection = new PlotPointCollection(200); //摩擦力坐标点集合
        internal static PlotPointCollection humPtCollection = new PlotPointCollection(200); //压力坐标点集合
        internal static PlotPointCollection tempPtCollection = new PlotPointCollection(200); //平台转速坐标点集合
        internal static PlotPointCollection motoTorPtCollection = new PlotPointCollection(200);//电机扭矩曲线集合
        internal static PlotPointCollection motoSpeedPtCollection = new PlotPointCollection(200);//电机转速曲线集合
        internal static PlotPointCollection fricSpeedPtCollection = new PlotPointCollection(200);//电机转速曲线集合
        
        internal EnumerableDataSource<PlotPoint> pressure;
        internal EnumerableDataSource<PlotPoint> platSpeed;
        internal EnumerableDataSource<PlotPoint> fricForce;
        internal EnumerableDataSource<PlotPoint> humidity;
        internal EnumerableDataSource<PlotPoint> temperature;
        internal EnumerableDataSource<PlotPoint> motoTor;
        internal EnumerableDataSource<PlotPoint> motoSpeed;
        internal EnumerableDataSource<PlotPoint> fricSpeed;
       
        //构造函数
        public PlotterData()
        {
            //以集合对象为实参，实例化曲线数据源,集合元素为<>内部类型的对象
            pressure = new EnumerableDataSource<PlotPoint>(presPtCollection);
            platSpeed = new EnumerableDataSource<PlotPoint>(platSpePtCollection);
            fricForce = new EnumerableDataSource<PlotPoint>(fricForPtCollection);
            humidity = new EnumerableDataSource<PlotPoint>(humPtCollection);
            temperature = new EnumerableDataSource<PlotPoint>(tempPtCollection);
            motoTor = new EnumerableDataSource<PlotPoint>(motoTorPtCollection);
            motoSpeed = new EnumerableDataSource<PlotPoint>(motoSpeedPtCollection);
            fricSpeed = new EnumerableDataSource<PlotPoint>(fricSpeedPtCollection);

            //设置坐标
            SetMap(platSpeed);
            SetMap(fricForce);
            SetMap(pressure);
            SetMap(humidity);
            SetMap(temperature);
            SetMap(motoTor);
            SetMap(motoSpeed);
            SetMap(fricSpeed);

        }
        //设置坐标
        void SetMap(EnumerableDataSource<PlotPoint> dataSource)
        {
            dataSource.SetXMapping(x => x.xLabel);
            dataSource.SetYMapping(y => y.yValue);
        }

    }
}
