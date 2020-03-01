import Vue from 'vue'
import ElementUI from 'element-ui'
import 'element-ui/lib/theme-chalk/index.css'
import VueRouter from 'vue-router'
import vueAxios from 'vue-axios'
import axios from 'axios'
Vue.prototype.$http = axios
axios.defaults.baseURL='https://localhost:10001/api/'

axios.interceptors.request.use(config=>{
  config.headers.Authorization = window.sessionStorage.getItem('token')
  console.log(config)
  return config
})

import App from './App.vue'

import router from './router/index'

Vue.config.productionTip = false

Vue.use(ElementUI)
Vue.use(VueRouter)
Vue.use(vueAxios, axios)

new Vue({
  router,
  render: h => h(App),
}).$mount('#app')
