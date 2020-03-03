<template>
  <div>
    <el-breadcrumb separator-class="el-icon-arrow-right">
      <el-breadcrumb-item :to="{path:'/'}">首页</el-breadcrumb-item>
      <el-breadcrumb-item>菜单列表</el-breadcrumb-item>
    </el-breadcrumb>

    <el-card>
      <el-row :gutter="20">
        <el-col :span="10">
          <el-input placeholder="输入关键字进行过滤" v-model="filterText"></el-input>
        </el-col>
        <el-col :span="5">
          <el-switch
            style="margin-bottom:0px"
            v-model="menuOnly"
            active-text="仅显示菜单"
            inactive-text
            @change="getMenus()"
          ></el-switch>
        </el-col>

        <el-col :span="9">
          <el-button type="primary" @click="addDialogFormVisible=true">新增顶级节点</el-button>
        </el-col>
      </el-row>
      <el-table
        :data="tableData.filter(data => !filterText || data.name.toLowerCase().includes(filterText.toLowerCase()))"
        style="width: 100%;margin-bottom: 20px;margin-top: 20px"
        :row-class-name="tableRowClassName"
        row-key="id"
        border
        default-expand-all
        :tree-props="{children: 'children', hasChildren: 'hasChildren'}"
      >
        <el-table-column prop="name" label="名称" sortable width="180">
          <template slot-scope="scope">
            <i :class="scope.row.icon"></i>
            <span style="margin-left:5px">{{scope.row.name}}</span>
          </template>
        </el-table-column>
        <el-table-column prop="hierarchyCode" label="权限点" sortable width="180"></el-table-column>
        <el-table-column prop="path" label="地址" sortable width="180"></el-table-column>
        <el-table-column prop="type" label="类型">
          <template slot-scope="scope">
            <el-tag type="success" v-show="scope.row.type==0">菜单</el-tag>
            <el-tag type="info" v-show="scope.row.type==1">功能</el-tag>
          </template>
        </el-table-column>
        <el-table-column prop="remarks" label="备注" sortable width="180"></el-table-column>
        <el-table-column label="操作">
          <template slot-scope="scope">
            <el-button
              type="text"
              v-show="scope.row.type==0"
              @click="openAddDialogue(scope.row.id)"
            >新增子节点</el-button>
            <el-button type="text" @click="openModifyDialogue(scope.row.id)">修改</el-button>
            <el-button type="text" v-show="ifEnd(scope.row)" @click="remove(scope.row.id)">删除</el-button>
            <el-button type="text" v-show="scope.row.type==0" @click="moveUp(scope.row.id,scope.row.type)">上移</el-button>
            <el-button type="text" v-show="scope.row.type==0" @click="moveDown(scope.row.id,scope.row.type)">下移</el-button>
          </template>
        </el-table-column>
      </el-table>

      <el-dialog title="添加权限" :visible.sync="addDialogFormVisible" @close="clearAddFields">
        <el-form :model="addForm" :rules="formRules" ref="addFormRef" label-width="80px">
          <el-form-item label="名称" prop="name">
            <el-input v-model="addForm.name"></el-input>
          </el-form-item>
          <el-form-item label="类型" prop="type">
            <el-radio v-model="addForm.type" label="0">菜单</el-radio>
            <el-radio v-model="addForm.type" label="1">功能</el-radio>
          </el-form-item>
          <el-form-item label="编码" prop="code">
            <el-input v-model="addForm.code"></el-input>
          </el-form-item>
          <el-form-item label="路径" prop="path">
            <el-input v-model="addForm.path"></el-input>
          </el-form-item>
          <el-form-item label="菜单图标" prop="icon">
            <el-input v-model="addForm.icon"></el-input>
          </el-form-item>
          <el-form-item label="备注" prop="remarks">
            <el-input
              v-model="addForm.remarks"
              type="textarea"
              :autosize="{ minRows: 2, maxRows: 4}"
              placeholder="请输入内容"
            ></el-input>
          </el-form-item>
        </el-form>
        <div slot="footer" class="dialog-footer">
          <el-button @click="addDialogFormVisible = false">取 消</el-button>
          <el-button type="primary" @click="addMenu">确 定</el-button>
        </div>
      </el-dialog>

      <el-dialog title="修改权限" :visible.sync="modifyDialogFormVisible" @close="clearModifyFields">
        <el-form :model="modifyForm" :rules="formRules" ref="modifyFormRef" label-width="80px">
          <el-form-item label="名称" prop="name">
            <el-input v-model="modifyForm.name"></el-input>
          </el-form-item>
          <el-form-item label="类型" prop="type">
            <el-radio v-model="modifyForm.type" label="0">菜单</el-radio>
            <el-radio v-model="modifyForm.type" label="1">功能</el-radio>
          </el-form-item>
          <el-form-item label="编码" prop="code">
            <el-input v-model="modifyForm.code"></el-input>
          </el-form-item>
          <el-form-item label="路径" prop="path">
            <el-input v-model="modifyForm.path"></el-input>
          </el-form-item>
          <el-form-item label="菜单图标" prop="icon">
            <el-input v-model="modifyForm.icon"></el-input>
          </el-form-item>
          <el-form-item label="备注" prop="remarks">
            <el-input
              v-model="modifyForm.remarks"
              type="textarea"
              :autosize="{ minRows: 2, maxRows: 4}"
              placeholder="请输入内容"
            ></el-input>
          </el-form-item>
        </el-form>
        <div slot="footer" class="dialog-footer">
          <el-button @click="modifyDialogFormVisible = false">取 消</el-button>
          <el-button type="primary" @click="modifyMenu">确 定</el-button>
        </div>
      </el-dialog>
    </el-card>
  </div>
</template>

<style lang="less">
.el-table .menu-row {
  background: white;
}

.el-table .function-row {
  background: white;
}
</style>

<script>
export default {
  created: function() {
    this.getMenus();
  },

  methods: {
    filterNode(value, data) {
      if (!value) return true;
      return data.label.indexOf(value) !== -1;
    },
    clearAddFields() {
      this.$refs.addFormRef.resetFields();
    },
    clearModifyFields() {
      this.$refs.modifyFormRef.resetFields();
    },
    openAddDialogue(parentId) {
      this.addDialogFormVisible = true;
      this.addForm.parentId = parentId;
    },
    async openModifyDialogue(id) {
      const { data: result } = await this.$http.get(`/menu/${id}`);
      this.modifyForm = result.data;
      this.modifyForm.type = this.modifyForm.type.toString();

      this.modifyDialogFormVisible = true;
    },
    remove(id) {
      this.$confirm("确认删除？")
        .then(async () => {
          const { data: result } = await this.$http.delete(`/menu/${id}`);
          if (result.code != 0) {
            this.$message.error(result.message);
          } else {
            this.$message.success("操作成功");
            this.getMenus();
          }
        })
        .catch(() => {});
    },
    addMenu() {
      this.$refs.addFormRef.validate(async valid => {
        if (!valid) return;
        this.addForm.type = parseInt(this.addForm.type);
        const { data: result } = await this.$http.post(
          "/menus",
          this.addForm
        );
        if (result.code == 0) {
          this.$message.success("操作成功");
          this.addDialogFormVisible = false;
          this.getMenus();
        } else {
          this.$message.error(result.message);
        }
      });
    },
    async modifyMenu() {
      this.modifyForm.type = parseInt(this.modifyForm.type);
      const { data: result } = await this.$http.put(
        `menu/${this.modifyForm.id}`,
        this.modifyForm
      );
      if (result.code != 0) {
        this.$message.error(result.message);
      } else {
        this.$message.success("操作成功");
        this.modifyDialogFormVisible = false;
        this.getMenus();
      }
    },
    editMenu() {},
    async getMenus() {
      let url = "menus";
      if (this.menuOnly) {
        url += "?scope=menu";
      }
      const { data: result } = await this.$http.get(url);
      if (result.code != 0) {
        this.$message.error(result.message);
      } else {
        this.tableData = result.data;
      }
    },
    ifEnd(data) {
      return data.children == null;
    },
    tableRowClassName({ row }) {
      if (row.type == 1) {
        return "menu-row";
      } else if (row.type == 2) {
        return "function-row";
      }
      return "";
    },
    async moveUp(id, type) {
      const { data: result } = await this.$http.put(
        `/menu/${id}/position?action=up`
      );
      if (result.code == 0) {
        if (type == 0) {
          window.location.reload();
        } else {
          this.getMenus();
        }
      }
    },
    async moveDown(id, type) {
      const { data: result } = await this.$http.put(
        `/menu/${id}/position?action=down`
      );
      if (result.code == 0) {
        if (type == 0) {
          window.location.reload();
        } else {
          this.getMenus();
        }
      }
    }
  },

  data() {
    return {
      menuOnly: true,
      filterText: "",
      addForm: {
        name: "",
        path: "",
        code: "",
        type: null,
        remarks: "",
        icon: ""
      },
      modifyForm: {},
      addDialogFormVisible: false,
      modifyDialogFormVisible: false,
      formRules: {
        name: [
          { required: true, message: "请输入名称", trigger: "blur" },
          {
            min: 1,
            max: 50,
            message: "名称长度在1~50个字符之间",
            trigger: "blur"
          }
        ],
        type: [{ required: true, message: "请选择类型", trigger: "blur" }],
        code: [
          { required: true, message: "请输入编码", trigger: "blur" },
          {
            min: 1,
            max: 50,
            message: "编码长度在1~50个字符之间",
            trigger: "blur"
          }
        ],
        path: [
          {
            max: 200,
            message: "地址长度不能大于200个字符",
            trigger: "blur"
          }
        ],
        remarks: [
          {
            max: 500,
            message: "备注长度不能大于500个字符",
            trigger: "blur"
          }
        ]
      },
      tableData: [],
      defaultProps: {
        children: "children",
        label: "name"
      }
    };
  }
};
</script>

<style lang="less" scoped>
.filter-tree {
  margin-top: 20px;
  width: 50%;
}

.custom-tree-node {
  flex: 1;
  display: flex;
  align-items: center;
  justify-content: space-between;
  font-size: 14px;
  padding-right: 8px;
}
</style>