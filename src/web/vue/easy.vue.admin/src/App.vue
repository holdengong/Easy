<template>
  <div id="app">
    <router-view></router-view>
  </div>
</template>

<script>
import Home from "./components/Home.vue";
import VueRouter from "vue-router";
import './assets/css/global.css'

export default {
  name: "App",
  router: routerObj
};

var routerObj = new VueRouter({
  routes: [
    { path: "/", component: Home }
  ]
});
</script>