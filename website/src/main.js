import Vue from 'vue'
import { BootstrapVue, IconsPlugin } from 'bootstrap-vue'
import 'bootstrap/dist/css/bootstrap.css'
import 'bootstrap-vue/dist/bootstrap-vue.css'
import VueRouter from 'vue-router'
import VueSession from 'vue-session'
import Vuex from 'vuex'
import store from 'vuex'
import VueShowdown from 'vue-showdown'
import App from './App.vue'
import routes from './routes';

Vue.use(Vuex) 

Vue.config.productionTip = true
Vue.prototype.$baseurl = (Vue.config.productionTip) ? 'https://seeshells.herokuapp.com/' : 'http://localhost:3000/'

Vue.use(BootstrapVue)
Vue.use(IconsPlugin)
Vue.use(VueRouter)
Vue.use(VueSession)
Vue.use(VueShowdown);

const router = new VueRouter({ mode: 'history', routes });

new Vue({
    router,
    store,
    render: h => h(App),
    created() {
        if (sessionStorage.redirect) {
            const redirect = sessionStorage.redirect
            delete sessionStorage.redirect
            this.$router.push(redirect)
        }
    }
}).$mount('#app');