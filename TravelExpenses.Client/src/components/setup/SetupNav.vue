<template>
  <div>
    <v-card>
      <v-list class="pa-0" three-line>
        <template v-for="(item, index) in items">
          <v-divider
            v-if="item.divider"
            :inset="item.inset"
            :key="index"
          ></v-divider>
          <v-list-tile
            v-else
            :key="item.title"
            avatar
            @click="navigate(item.window)"
          >
            <v-list-tile-avatar size="55" class="mr-3 mt-1">
              <v-icon large class="primary white--text">{{ item.icon }}</v-icon>
            </v-list-tile-avatar>

            <v-list-tile-content>
              <v-list-tile-title v-html="item.title"></v-list-tile-title>
              <v-list-tile-sub-title
                v-html="item.subtitle"
              ></v-list-tile-sub-title>
            </v-list-tile-content>
          </v-list-tile>
        </template>
      </v-list>
    </v-card>
  </div>
</template>

<script>
import SetupWindow from '@/common/enums/SetupWindows.js'

export default {
  data() {
    return {
      items: [
        {
          window: SetupWindow.locations,
          icon: 'add_location',
          title: 'Locations',
          subtitle: 'Add the locations you will be travelling to'
        },
        { divider: true, inset: true },
        {
          window: SetupWindow.categories,
          icon: 'collections_bookmark',
          title: 'Categories',
          subtitle: 'Add categories that will be used to group transactions'
        },
        { divider: true, inset: true },
        {
          window: SetupWindow.keywords,
          icon: 'my_location',
          title: 'Keywords',
          subtitle:
            'Add keywords that can be used to enable querying of transactions'
        }
      ]
    }
  },
  methods: {
    navigate(window) {
      this.$store.dispatch('SetupData/setSetupWindow', window)
    }
  }
}
</script>

<style scoped></style>
