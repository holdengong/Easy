<template>
  <div id="app">
    <router-view></router-view>
    <router-link to="/login">登录</router-link>
    <router-link to="/register">注册</router-link>
  </div>
</template>

<script>
import Login from "./components/Login.vue";
import VueRouter from "vue-router";

export default {
  name: "App",
  router: routerObj
};

var routerObj = new VueRouter({
  routes: [
    { path: "/", redirect: "/login" },
    { path: "/login", component: Login }
  ]
});
</script>

<style>
#app {
  font-family: Avenir, Helvetica, Arial, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  text-align: center;
  color: #2c3e50;
  margin-top: 60px;
}
</style>
