#!/bin/bash

echo "Установка Orca Patcher";
mkdir build
cd build
cmake ..
make
cp -f OrcaPatcher ../
cd ..
rm -rf build
echo "Установка Orca Patcher завершена";