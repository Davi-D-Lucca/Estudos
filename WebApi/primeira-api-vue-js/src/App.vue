<template>
  <EmployeesPage v-if="logged" @logout="logout" />
  <LoginPage v-else @logged="logged = true" />
</template>

<script setup>
import { ref, onMounted, onUnmounted } from 'vue';
import LoginService from '@/services/LoginService';
import LoginPage from '@/components/LoginPage.vue';
import EmployeesPage from '@/components/EmployeesPage.vue';

let _loginService = new LoginService();
let logged = ref(_loginService.IsLogged());

function logout(){
  _loginService.Logout();
  logged.value = false;
}

// Disparado pelo http-common quando a API responde 401
const onExpired = () => { logged.value = false; };
onMounted(() => window.addEventListener("logout", onExpired));
onUnmounted(() => window.removeEventListener("logout", onExpired));
</script>
