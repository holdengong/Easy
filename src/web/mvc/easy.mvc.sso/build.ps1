# 定义变量
$vueAppRootPath = "../../vue/easy.vue.sso"; #vue根目录
$distPath = $vueAppRootPath + "/dist"; #dist静态文件目录
$nodemodulePath = $vueAppRootPath + "/node_modules"; #node_modules目录
$indexHtmlPath = $distPath+"/index.html"; # 前端首页地址
$indexCshtmlPath = "Views/Home/Index.cshtml"; # 后端首页地址

# 前端操作
cd $vueAppRootPath

# 删除dist文件夹
if(Test-Path $distPath)
{
    rmdir $distPath -Force -Recurse
} 

#前端构建
try {
    npm run build
}
catch {
    # 删除node_modules文件夹
    if(Test-Path $nodemodulePath)
    {
        rmdir $nodemodulePath -Force -Recurse
    }
    #npm 装包
    cd $vueAppRootPath
    npm install
}

# 后端操作
cd "../../mvc/easy.mvc.sso"

# 拷贝dist到wwwroot
$distAll = $distPath + "/*";
cp $distAll "wwwroot" -Recurse -Force
cp $indexHtmlPath $indexCshtmlPath -Force

dotnet restore
dotnet run