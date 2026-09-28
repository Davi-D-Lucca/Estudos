<template>
  <div class="login-page flex flex-center">
    <q-card class="login-card q-pa-md">
      <q-card-section class="text-center">
        <img alt="Vue logo" src="../assets/logo.png" width="72">
        <div class="text-h6 q-mt-sm">PrimeiraApi</div>
        <div class="text-caption text-grey-7">Entre para gerenciar os funcionários</div>
      </q-card-section>

      <q-card-section>
        <form @submit.prevent="authenticate">
          <q-input v-model="data.username" outlined dense label="Usuário" autofocus>
            <template #prepend><q-icon name="person" /></template>
          </q-input>
          <q-input v-model="data.password" type="password" outlined dense label="Senha" class="q-mt-md">
            <template #prepend><q-icon name="lock" /></template>
          </q-input>
          <q-btn type="submit" color="primary" label="Entrar" class="full-width q-mt-lg" :loading="loading" />
        </form>
      </q-card-section>
    </q-card>
  </div>
</template>

<script setup>
import { ref } from 'vue';
import { useQuasar } from 'quasar';
import LoginService from '@/services/LoginService';

const emit = defineEmits(["logged"]);
const $q = useQuasar();

let _loginService = new LoginService();
let loading = ref(false);

let data = ref({
    username: "",
    password: "",
})

async function authenticate(){
  loading.value = true;
  try {
    await _loginService.Login(data.value);
    emit("logged");
  } catch {
    $q.notify({ type: "negative", message: "Usuário ou senha inválidos" });
  } finally {
    loading.value = false;
  }
}
</script>

<style scoped>
.login-page {
  min-height: 100vh;
  background: linear-gradient(135deg, #027be3 0%, #26a69a 100%);
}
.login-card {
  width: 100%;
  max-width: 360px;
}
</style>
