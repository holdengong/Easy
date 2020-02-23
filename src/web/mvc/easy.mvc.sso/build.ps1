# 定义变量
$vueAppRootPath = "../../vue/easy.vue.sso"; #vue根目录
$distPath = $vueAppRootPath + "/dist"; #dist静态文件目录
$nodemodulePath = $vueAppRootPath + "/node_modules"; #node_modules目录
$indexHtmlPath = $distPath+"/index.html"; # 前端首页地址
$indexCshtmlPath = "Views/Home/Index.cshtml"; # 后端首页地址
$errorcode = 0;

function stopWhenError($errmsg)
{
    if($LASTEXITCODE -gt 0)
    {
        Write-Error $errmsg
        exit
    }
}

Set-Location $vueAppRootPath
# 删除dist文件夹
if(Test-Path $distPath)
{
    Remove-Item $distPath -Force -Recurse
} 

#前端构建
npm run build

$errorcode = $LASTEXITCODE

if($errorcode -gt 0)
{
    Write-Host "npm run build failed, trying npm install..."
    # 删除node_modules文件夹
    if(Test-Path $nodemodulePath)
    {
        Remove-Item $nodemodulePath -Force -Recurse
    }
    npm install

    stopWhenError "npm install failed...please check manually..."

    npm run build
    stopWhenError "npm build still failed...please check manually..."
}

Set-Location "../../mvc/easy.mvc.sso"

# 后端操作
# 拷贝dist到wwwroot
$distAll = $distPath + "/*";
Copy-Item $distAll "wwwroot" -Recurse -Force

Copy-Item $indexHtmlPath $indexCshtmlPath -Force

dotnet restore
dotnet run