import Vue from 'vue'
import App from './App.vue'

import router from './router'
import vuetify from './plugins/vuetify';
import axios from 'axios';

import GlobalMixin from "./mixins/global-mixin";

// NOTE: the baseURL value is actually a proxy to something else. Look at vue.config.js for actual URL
// axios.defaults.baseURL = "http://localhost:5000/"
Vue.prototype.$http = axios;

Vue.config.productionTip = false;

Vue.mixin(GlobalMixin);

new Vue({
  router,
  vuetify,
  render: h => h(App)
}).$mount('#app')