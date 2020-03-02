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