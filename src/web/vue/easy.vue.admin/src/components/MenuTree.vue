<template>
  <div>
    <label v-for="menu in data" :key="menu.id">
      <el-submenu :index="menu.id" v-if="menu.children">
        <template slot="title">
          <i :class="menu.icon"></i>
          <span>{{menu.name}}</span>
        </template>
        <label>
          <menutree :data="menu.children"></menutree>
        </label>
      </el-submenu>
      <el-menu-item v-else-if="menu.path && menu.path.includes('http')" index>
        <i :class="menu.icon"></i>
        <span slot="title">
          <a :href="menu.path" target="_blank" style="text-decoration:none">{{menu.name}}</a>
        </span>
      </el-menu-item>
      <el-menu-item v-else :index="menu.path">
        <i :class="menu.icon"></i>
        <span slot="title">{{menu.name}}</span>
      </el-menu-item>
    </label>
  </div>
</template>

<script>
import menutree from "../components/MenuTree";
export default {
  name: "menutree",
  data() {
    return {
      menu_data: []
    };
  },
  components: {
    menutree: menutree
  },
  props: ["data"],
  methods: {}
};
</script>

<style lang="less" scoped>
a {
  text-decoration: none;
  color: rgb(255, 255, 255);
  background-color: rgb(51, 55, 68);
}
</style>