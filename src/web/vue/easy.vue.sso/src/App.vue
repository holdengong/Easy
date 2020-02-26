<template>
  <div id="app">
    <router-view></router-view>
  </div>
</template>

<script>
import Login from "./components/Login.vue";
import VueRouter from "vue-router";
import './assets/css/global.css'

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