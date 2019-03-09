<template>
  <div>
    <h2 class="white--text">Added keywords</h2>
    <v-list subheader>
      <v-list-tile v-if="busy">
        <v-list-tile-content>
          <v-progress-circular
            indeterminate
            color="primary"
          ></v-progress-circular>
        </v-list-tile-content>
      </v-list-tile>
      <v-list-tile v-else-if="listItems.length < 1">
        <v-list-tile-content>
          <v-list-tile-title
            >You currently don't have any keywords added</v-list-tile-title
          >
        </v-list-tile-content>
      </v-list-tile>
      <template v-for="(item, index) in listItems">
        <v-list-tile @click="edit(item)" :key="item.id">
          <v-list-tile-content>
            <v-list-tile-title v-text="item.keywordName"></v-list-tile-title>
          </v-list-tile-content>
        </v-list-tile>
        <v-divider
          v-if="index + 1 < listItems.length"
          class="my-0"
          :key="item.id + 'div'"
        ></v-divider>
      </template>
    </v-list>
    <div class="spacer"></div>
    <div class="controls pa-2">
      <v-window touchless v-model="editWindow">
        <v-window-item>
          <add-keyword @cancel="cancelAdd" />
        </v-window-item>
        <v-window-item>
          <edit-keyword :keyword="selectedKeyword" @cancel="cancelEdit" />
        </v-window-item>
      </v-window>
    </div>
  </div>
</template>

<script>
import orderBy from 'lodash/orderBy'
import clone from 'lodash/clone'
import AddKeyword from '@/components/setup/keyword/AddKeyword.vue'
import EditKeyword from '@/components/setup/keyword/EditKeyword.vue'
import SetupWindows from '@/common/enums/SetupWindows.js'

export default {
  data() {
    return {
      editWindow: 0,
      selectedKeyword: {}
    }
  },
  components: {
    AddKeyword,
    EditKeyword
  },
  methods: {
    edit(item) {
      this.selectedKeyword = clone(item)
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
      let keywords = this.$store.state.Keyword.keywords
      return orderBy(keywords, [c => c.keywordName.toLowerCase()])
    },
    busy() {
      return this.$store.state.Keyword.busy
    }
  }
}
</script>

<style scoped>
.spacer {
  height: 175px;
}

.controls {
  position: fixed;
  width: 100%;
  bottom: 56px;
  margin-left: -16px;
  background: rgba(0, 0, 0, 0.8);
}
</style>
