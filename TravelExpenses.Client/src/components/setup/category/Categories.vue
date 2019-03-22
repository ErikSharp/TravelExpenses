<template>
  <div>
    <v-progress-circular
      v-if="busy"
      indeterminate
      color="primary"
    ></v-progress-circular>
    <v-card v-else-if="listItems.length < 1"
      >You currently don't have any categories added</v-card
    >
    <v-card
      v-else
      v-for="(item, index) in listItems"
      :key="item.id"
      class="mb-1"
      @click="edit(item)"
    >
      <v-layout align-center justify-start row fill-height>
        <v-flex shrink>
          <v-avatar size="40" class="ma-2 elevation-5" :color="getColor(item)">
            <v-icon size="25" class="white--text">{{ item.icon }}</v-icon>
          </v-avatar>
        </v-flex>
        <v-flex>{{ item.categoryName }}</v-flex>
      </v-layout>
    </v-card>
    <div class="spacer"></div>
    <div class="controls pa-2">
      <v-window touchless v-model="editWindow">
        <v-window-item>
          <add-category @cancel="cancelAdd" />
        </v-window-item>
        <v-window-item>
          <edit-category @cancel="cancelEdit" />
        </v-window-item>
      </v-window>
    </div>
  </div>
</template>

<script>
import orderBy from 'lodash/orderBy'
import clone from 'lodash/clone'
import AddCategory from '@/components/setup/category/AddCategory.vue'
import EditCategory from '@/components/setup/category/EditCategory.vue'
import SetupWindows from '@/common/enums/SetupWindows.js'

export default {
  data() {
    return {
      editWindow: 0
    }
  },
  components: {
    AddCategory,
    EditCategory
  },
  methods: {
    getColor(item) {
      const HTMLcolor = item.color.toString(16)
      return '#000000'.substring(0, 7 - HTMLcolor.length) + HTMLcolor
    },
    edit(item) {
      this.$store.dispatch('Category/setEditCategory', item)
      this.editWindow = 1
    },
    cancelAdd() {
      this.$store.dispatch('SetupData/setSetupWindow', SetupWindows.navigation)
    },
    cancelEdit() {
      this.editWindow = 0
    }
  },
  computed: {
    listItems() {
      let categories = this.$store.state.Category.categories
      return orderBy(categories, [c => c.categoryName.toLowerCase()])
    },
    busy() {
      return this.$store.state.Category.busy
    }
  }
}
</script>

<style scoped>
.spacer {
  height: 174px;
}

.controls {
  position: fixed;
  width: 100%;
  bottom: 56px;
  margin-left: -16px;
  background: rgba(0, 0, 0, 0.8);
}
</style>
