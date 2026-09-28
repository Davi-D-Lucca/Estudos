<template>
  <q-layout view="hHh lpR fFf">
    <q-header elevated class="bg-primary">
      <q-toolbar>
        <q-toolbar-title>PrimeiraApi · Funcionários</q-toolbar-title>
        <q-btn-toggle v-model="version" :options="versions" toggle-color="white" toggle-text-color="primary"
          color="primary" text-color="white" unelevated dense class="q-mr-md" />
        <q-btn flat icon="logout" label="Sair" @click="emit('logout')" />
      </q-toolbar>
    </q-header>

    <q-page-container>
      <q-page class="q-pa-md">
        <div class="row q-col-gutter-md q-mb-md items-center">
          <div class="col-12 col-sm-4">
            <q-input v-model.number="searchId" type="number" outlined dense label="Buscar por ID"
              @keyup.enter="search" clearable @clear="load">
              <template #append><q-btn flat round dense icon="search" @click="search" /></template>
            </q-input>
          </div>
          <div class="col-12 col-sm-8 text-right">
            <q-btn color="primary" icon="add" label="Novo funcionário" @click="openAdd" />
          </div>
        </div>

        <q-table :rows="rows" :columns="columns" row-key="id" :loading="loading" flat bordered
          hide-pagination :rows-per-page-options="[0]" no-data-label="Nenhum funcionário encontrado">
          <template #body-cell-actions="props">
            <q-td :props="props" class="text-right">
              <q-btn flat dense color="primary" icon="image" label="Foto" @click="showPhoto(props.row)" />
            </q-td>
          </template>
        </q-table>

        <div class="row items-center justify-end q-mt-md q-gutter-sm" v-if="!searching">
          <q-select v-model="pageQuantity" :options="[5, 10, 20]" dense outlined label="Por página"
            style="width: 110px" @update:model-value="goToPage(0)" />
          <q-btn flat round icon="chevron_left" :disable="pageNumber === 0" @click="goToPage(pageNumber - 1)" />
          <span>Página {{ pageNumber + 1 }}</span>
          <q-btn flat round icon="chevron_right" :disable="rows.length < pageQuantity" @click="goToPage(pageNumber + 1)" />
        </div>
      </q-page>
    </q-page-container>

    <!-- Cadastro -->
    <q-dialog v-model="addDialog">
      <q-card style="width: 400px; max-width: 90vw">
        <q-card-section class="text-h6">Novo funcionário</q-card-section>
        <q-form @submit="save">
          <q-card-section class="q-gutter-md">
            <q-input v-model="form.name" outlined dense label="Nome" :rules="[v => !!v || 'Informe o nome']" />
            <q-input v-model.number="form.age" type="number" outlined dense label="Idade"
              :rules="[v => v > 0 || 'Informe a idade']" />
            <q-file v-model="form.photo" outlined dense label="Foto" accept="image/*"
              :rules="[v => !!v || 'Selecione uma foto']">
              <template #prepend><q-icon name="attach_file" /></template>
            </q-file>
          </q-card-section>
          <q-card-actions align="right">
            <q-btn flat label="Cancelar" v-close-popup />
            <q-btn type="submit" color="primary" label="Salvar" :loading="saving" />
          </q-card-actions>
        </q-form>
      </q-card>
    </q-dialog>

    <!-- Foto -->
    <q-dialog v-model="photoDialog" @hide="clearPhoto">
      <q-card style="width: 420px; max-width: 90vw">
        <q-card-section class="row items-center">
          <div class="text-h6">{{ photo.name }}</div>
          <q-space />
          <q-btn flat round dense icon="close" v-close-popup />
        </q-card-section>
        <q-card-section class="flex flex-center" style="min-height: 200px">
          <q-spinner v-if="!photo.url" size="3em" color="primary" />
          <img v-else :src="photo.url" style="max-width: 100%; max-height: 60vh" />
        </q-card-section>
        <q-card-actions align="right" v-if="photo.url">
          <q-btn flat color="primary" icon="download" label="Baixar" :href="photo.url"
            :download="`funcionario-${photo.id}.png`" />
        </q-card-actions>
      </q-card>
    </q-dialog>
  </q-layout>
</template>

<script setup>
import { ref, computed, watch, onMounted } from 'vue';
import { useQuasar } from 'quasar';
import EmployeeService from '@/services/EmployeeService';

const emit = defineEmits(["logout"]);
const $q = useQuasar();

const versions = [
  { label: "v1", value: "v1" },
  { label: "v2", value: "v2" },
];
let version = ref("v1");
const _employeeService = computed(() => new EmployeeService(version.value));

const columns = [
  { name: "id", label: "ID", field: "id", align: "left", style: "width: 80px" },
  { name: "nameEmployee", label: "Nome", field: "nameEmployee", align: "left" },
  { name: "photo", label: "Arquivo da foto", field: "photo", align: "left" },
  { name: "actions", label: "", field: "id" },
];

let rows = ref([]);
let loading = ref(false);
let pageNumber = ref(0);
let pageQuantity = ref(10);
let searchId = ref(null);
let searching = ref(false);

async function load(){
  loading.value = true;
  searching.value = false;
  try {
    rows.value = await _employeeService.value.Get(pageNumber.value, pageQuantity.value);
  } catch {
    $q.notify({ type: "negative", message: "Erro ao carregar funcionários" });
  } finally {
    loading.value = false;
  }
}

function goToPage(page){
  pageNumber.value = page;
  load();
}

async function search(){
  if (!searchId.value) return load();
  loading.value = true;
  searching.value = true;
  try {
    const employee = await _employeeService.value.Search(searchId.value);
    rows.value = employee ? [employee] : [];
  } catch {
    $q.notify({ type: "negative", message: "Erro ao buscar funcionário" });
  } finally {
    loading.value = false;
  }
}

// Cadastro
let addDialog = ref(false);
let saving = ref(false);
let form = ref({ name: "", age: null, photo: null });

function openAdd(){
  form.value = { name: "", age: null, photo: null };
  addDialog.value = true;
}

async function save(){
  saving.value = true;
  try {
    await _employeeService.value.Add(form.value);
    addDialog.value = false;
    $q.notify({ type: "positive", message: "Funcionário cadastrado" });
    load();
  } catch {
    $q.notify({ type: "negative", message: "Erro ao cadastrar funcionário" });
  } finally {
    saving.value = false;
  }
}

// Foto
let photoDialog = ref(false);
let photo = ref({ id: null, name: "", url: null });

async function showPhoto(row){
  photo.value = { id: row.id, name: row.nameEmployee, url: null };
  photoDialog.value = true;
  try {
    photo.value.url = await _employeeService.value.DownloadPhoto(row.id);
  } catch {
    photoDialog.value = false;
    $q.notify({ type: "negative", message: "Não foi possível carregar a foto" });
  }
}

function clearPhoto(){
  if (photo.value.url) URL.revokeObjectURL(photo.value.url);
  photo.value.url = null;
}

watch(version, () => goToPage(0));
onMounted(load);
</script>
