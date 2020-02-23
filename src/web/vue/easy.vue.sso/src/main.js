import Vue from 'vue'
import ElementUI from 'element-ui';
import 'element-ui/lib/theme-chalk/index.css';
import VueRouter from 'vue-router'
import vueAxios from 'vue-axios'
import axios from 'axios'

import App from './App.vue'

Vue.config.productionTip = false

Vue.use(ElementUI)
Vue.use(VueRouter)
Vue.use(vueAxios, axios)

new Vue({
  render: h => h(App),
}).$mount('#app')
