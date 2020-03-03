<template>
  <el-container>
    <!-- 头部区域 -->
    <el-header>
      <div>
        <img src alt />
        <span>Easy后台管理系统</span>
      </div>
      <el-button type="info" @click="logout">退出</el-button>
    </el-header>
    <!-- 主体区域 -->
    <el-container>
      <!-- 侧边栏 -->
      <el-aside :width="isMenuCollapse?'64px':'200px'">
        <!-- <div class="toggle-button" @click="toggleButtonClick">|||</div> -->
        <template>
          <div class="left">
            <el-menu  background-color="#333744" text-color="#fff" active-text-color="#ffd04b"
        :collapse="isMenuCollapse" :collapse-transition="false" router 
        :default-active="activePath">
              <menutree :data="menuData"></menutree>
            </el-menu>
          </div>
        </template>

        <!-- <el-menu background-color="#333744" text-color="#fff" active-text-color="#ffd04b"
        unique-opened :collapse="isMenuCollapse" :collapse-transition="false" router 
        :default-active="activePath">
            <el-submenu :index="item.id" v-for='item in menuList' :key='item.id'>
                <template slot="title">
                  <i :class="iconsObj[item.name]"></i>
                  <span>{{item.name}}</span>
                </template>

                <el-menu-item :index="subitem.path" v-for="subitem in item.children" :key='subitem.id'
                 @click="saveNavState(subitem.path)">
                  <template slot="title">
                     <i class="el-icon-menu"></i>
                     <span>{{subitem.name}}</span>
                  </template>
                </el-menu-item>
            </el-submenu>
        </el-menu>-->
      </el-aside>
      <!-- 内容区域 -->
      <el-main>
        <router-view></router-view>
      </el-main>
    </el-container>
  </el-container>
</template>

<script>
import menutree from "../components/MenuTree";
export default {
  async mounted() {
    const { data: result } = await this.$http.get("/menus?scope=menu");
    this.menuData = result.data;
  },
  data() {
    return {
      menuList: [],
      isMenuCollapse: false,
      activePath: "",
      menuData: []
    };
  },
  created() {
    this.getMenuList();
    this.activePath = window.sessionStorage.getItem("activePath");
  },
  methods: {
    logout() {
      window.sessionStorage.clear();
      window.location.href = "https://localhost:10001/extension/logout";
    },
    async getMenuList() {
      const { data: result } = await this.$http.get("menus");
      if (result.code != 0) {
        return this.$message.error(result.message);
      }
      console.log(result);
      this.menuList = result.data;
    },
    toggleButtonClick() {
      this.isMenuCollapse = !this.isMenuCollapse;
    },
    saveNavState(activePath) {
      window.sessionStorage.setItem("activePath", activePath);
      this.activePath = activePath;
    }
  },
  components: { menutree: menutree }
};
</script>

<style lang="less" scoped>
.el-header {
  background-color: #373d41;
  display: flex;
  justify-content: space-between;
  padding-left: 0;
  align-items: center;
  color: #fff;
  font-size: 20px;
  > div {
    display: flex;
    align-items: center;
    span {
      margin-left: 15px;
    }
  }
}

.el-aside {
  background-color: #333744;
  .el-menu {
    border-right: none;
  }
}

.el-main {
  background-color: #eaedf1;
}

.el-container {
  height: 100%;
}

.toggle-button {
  background-color: #4a5064;
  font-size: 10px;
  line-height: 24px;
  color: #fff;
  text-align: center;
  letter-spacing: 0.2em;
  cursor: pointer;
}
</style>