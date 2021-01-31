import Vue from 'vue'
import VueRouter from 'vue-router'
import MainPage from '@/views/MainPage'
import ItemCollection from "@/views/ItemCollection";

Vue.use(VueRouter)

const routes = [
  {
    path: '/',
    name: 'MainPage',
    component: MainPage
  },
  {
    path: '/items',
    name: 'ItemCollection',
    component: ItemCollection
  }
]

const router = new VueRouter({
  mode: 'history',
  base: process.env.BASE_URL,
  routes
})

export default router
