﻿using System;
using KelpNet;
using KelpNet.Functions.Activations;
using KelpNet.Functions.Connections;
using KelpNet.Functions.Poolings;
using KelpNet.Loss;

namespace KelpNetTester.Tests
{
    //エクセルCNNの再現
    class Test5
    {
        public static void Run()
        {
            //各初期値を記述
            var initial_W1 = new[, , ,]
                {
                    {{{1.0,  0.5, 0.0}, { 0.5, 0.0, -0.5}, {0.0, -0.5, -1.0}}},
                    {{{0.0, -0.1, 0.1}, {-0.3, 0.4,  0.7}, {0.5, -0.2,  0.2}}}
                };
            var initial_b1 = new[] { 0.5, 1.0 };

            var initial_W2 = new[, , ,]
                {
                    {{{-0.1,  0.6}, {0.3, -0.9}}, {{ 0.7, 0.9}, {-0.2, -0.3}}},
                    {{{-0.6, -0.1}, {0.3,  0.3}}, {{-0.5, 0.8}, { 0.9,  0.1}}}
                };
            var initial_b2 = new[] { 0.1, 0.9 };

            var initial_W3 = new[,]
                {
                    {0.5, 0.3, 0.4, 0.2, 0.6, 0.1, 0.4, 0.3},
                    {0.6, 0.4, 0.9, 0.1, 0.5, 0.2, 0.3, 0.4}
                };
            var initial_b3 = new[] { 0.01, 0.02 };

            var initial_W4 = new[,] { { 0.8, 0.2 }, { 0.4, 0.6 } };
            var initial_b4 = new[] { 0.02, 0.01 };


            //入力データ
            var x = new[, ,]  {{
                    { 0.0, 0.0, 0.0, 0.0, 0.0, 0.2, 0.9, 0.2, 0.0, 0.0, 0.0, 0.0},
                    { 0.0, 0.0, 0.0, 0.0, 0.2, 0.8, 0.9, 0.1, 0.0, 0.0, 0.0, 0.0},
                    { 0.0, 0.0, 0.0, 0.1, 0.8, 0.5, 0.8, 0.1, 0.0, 0.0, 0.0, 0.0},
                    { 0.0, 0.0, 0.0, 0.3, 0.3, 0.1, 0.7, 0.2, 0.0, 0.0, 0.0, 0.0},
                    { 0.0, 0.0, 0.0, 0.1, 0.0, 0.1, 0.7, 0.2, 0.0, 0.0, 0.0, 0.0},
                    { 0.0, 0.0, 0.0, 0.0, 0.0, 0.1, 0.7, 0.1, 0.0, 0.0, 0.0, 0.0},
                    { 0.0, 0.0, 0.0, 0.0, 0.0, 0.4, 0.8, 0.1, 0.0, 0.0, 0.0, 0.0},
                    { 0.0, 0.0, 0.0, 0.0, 0.0, 0.8, 0.4, 0.1, 0.0, 0.0, 0.0, 0.0},
                    { 0.0, 0.0, 0.0, 0.0, 0.2, 0.8, 0.3, 0.0, 0.0, 0.0, 0.0, 0.0},
                    { 0.0, 0.0, 0.0, 0.0, 0.1, 0.8, 0.2, 0.0, 0.0, 0.0, 0.0, 0.0},
                    { 0.0, 0.0, 0.0, 0.0, 0.1, 0.7, 0.2, 0.0, 0.0, 0.0, 0.0, 0.0},
                    { 0.0, 0.0, 0.0, 0.0, 0.0, 0.3, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0}
                }};

            //教師信号
            double[] t = { 0.0, 1.0 };


            //層の中身をチェックしたい場合は、層単体でインスタンスを持つ
            var l2 = new Convolution2D(1, 2, 3, initialW: initial_W1, initialb: initial_b1);

            //ネットワークの構成を FunctionStack に書き連ねる
            FunctionStack nn = new FunctionStack(                
                l2, //new Convolution2D(1, 2, 3, initialW: initial_W1, initialb: initial_b1),
                new ReLU(),
                //new AveragePooling(2, 2),
                new MaxPooling(2, 2),
                new Convolution2D(2, 2, 2, initialW: initial_W2, initialb: initial_b2),
                new ReLU(),
                //new AveragePooling(2, 2),
                new MaxPooling(2, 2),
                new Linear(8, 2, initialW: initial_W3, initialb: initial_b3),
                new ReLU(),
                new Linear(2, 2, initialW: initial_W4, initialb: initial_b4)                
            );

            //optimizerの宣言を省略するとデフォルトのSGD(0.1)が使用される
            //nn.Optimizer = new SGD();

            //訓練を実施
            nn.Train(x, t, LossFunctions.MeanSquaredError);

            //Updateを実行するとgradが消費されてしまうため値を先に出力
            Console.WriteLine("gw1");
            Console.WriteLine(l2.gW);

            Console.WriteLine("gb1");
            Console.WriteLine(l2.gb);

            //更新
            nn.Update();

            Console.WriteLine("w1");
            Console.WriteLine(l2.W);

            Console.WriteLine("b1");
            Console.WriteLine(l2.b);
        }
    }
}