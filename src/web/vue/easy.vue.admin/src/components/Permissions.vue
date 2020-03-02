<template>
  <div>
    <el-breadcrumb separator-class="el-icon-arrow-right">
      <el-breadcrumb-item :to="{path:'/'}">首页</el-breadcrumb-item>
      <el-breadcrumb-item>权限管理</el-breadcrumb-item>
      <el-breadcrumb-item>权限列表</el-breadcrumb-item>
    </el-breadcrumb>

    <el-card>
      <el-row :gutter="20">
        <el-col :span="12">
          <el-input placeholder="输入关键字进行过滤" v-model="filterText"></el-input>
        </el-col>
        <el-col :span="12">
          <el-button type="primary" @click="addDialogFormVisible=true">新增顶级权限</el-button>
        </el-col>
      </el-row>
      <el-tree
        class="filter-tree"
        :data="data"
        :props="defaultProps"
        default-expand-all
        :filter-node-method="filterNode"
        ref="tree"
      >
        <span class="custom-tree-node" slot-scope="{ node, data }">
          <span>{{ node.label }}</span>
          <span>
            <el-button type="text" size="mini" @click.stop="() => openAddDialogue(data)">添加子项</el-button>
            <el-button type="text" size="mini" @click.stop="() => openModifyDialogue(node, data)">编辑</el-button>
            <el-button v-show=ifEnd(data) type="text" size="mini" @click.stop="() => remove(node, data)">删除</el-button>
          </span>
        </span>
      </el-tree>

      <el-dialog title="添加权限" :visible.sync="addDialogFormVisible" @close="clearFields">
        <el-form :model="form" :rules="formRules" ref="formRef" label-width="80px">
          <el-form-item label="名称" prop="name">
            <el-input v-model="form.name"></el-input>
          </el-form-item>
          <el-form-item label="类型" prop="type">
            <el-radio v-model="form.type" label="1">菜单</el-radio>
            <el-radio v-model="form.type" label="2">功能</el-radio>
          </el-form-item>
          <el-form-item label="编码" prop="code">
            <el-input v-model="form.code"></el-input>
          </el-form-item>
          <el-form-item label="路径" prop="path">
            <el-input v-model="form.path"></el-input>
          </el-form-item>
          <el-form-item label="备注" prop="remarks">
            <el-input
              v-model="form.remarks"
              type="textarea"
              :autosize="{ minRows: 2, maxRows: 4}"
              placeholder="请输入内容"
            ></el-input>
          </el-form-item>
        </el-form>
        <div slot="footer" class="dialog-footer">
          <el-button @click="addDialogFormVisible = false">取 消</el-button>
          <el-button type="primary" @click="addPermission">确 定</el-button>
        </div>
      </el-dialog>

      <el-dialog title="修改权限" :visible.sync="modifyDialogFormVisible" @close="clearFields">
        <el-form :model="form" :rules="formRules" ref="formRef" label-width="80px">
          <el-form-item label="名称" prop="name">
            <el-input v-model="form.name"></el-input>
          </el-form-item>
          <el-form-item label="类型" prop="type">
            <el-radio v-model="form.type" label="1">菜单</el-radio>
            <el-radio v-model="form.type" label="2">功能</el-radio>
          </el-form-item>
          <el-form-item label="编码" prop="code">
            <el-input v-model="form.code"></el-input>
          </el-form-item>
          <el-form-item label="路径" prop="path">
            <el-input v-model="form.path"></el-input>
          </el-form-item>
          <el-form-item label="备注" prop="remarks">
            <el-input
              v-model="form.remarks"
              type="textarea"
              :autosize="{ minRows: 2, maxRows: 4}"
              placeholder="请输入内容"
            ></el-input>
          </el-form-item>
        </el-form>
        <div slot="footer" class="dialog-footer">
          <el-button @click="modifyDialogFormVisible = false">取 消</el-button>
          <el-button type="primary" @click="modifyPermission">确 定</el-button>
        </div>
      </el-dialog>
    </el-card>
  </div>
</template>

<script>
export default {
  watch: {
    filterText(val) {
      this.$refs.tree.filter(val);
    }
  },

  created: function() {
    this.getPermissions();
  },

  methods: {
    filterNode(value, data) {
      if (!value) return true;
      return data.label.indexOf(value) !== -1;
    },
    clearFields() {
      this.$refs.formRef.resetFields();
    },
    openAddDialogue(data) {
      this.addDialogFormVisible = true;
      this.form.parentId = data.id;
      console.log(data);
    },
    async openModifyDialogue(node, rowData) {
      const { data: result } = await this.$http.get(
        `/permission/${rowData.id}`
      );
      this.form.id = rowData.id;
      this.form.name = result.data.name;
      this.form.path = result.data.path;
      this.form.code = result.data.code;
      this.form.type = result.data.type.toString();
      this.form.remarks = result.data.remarks;

      this.modifyDialogFormVisible = true;
    },
    remove(node, data) {
      this.$confirm("确认删除？")
        .then(async() => {
          const { data: result } = await this.$http.delete(
            `/permission/${data.id}`
          );
          if (result.code != 0) {
            this.$message.error(result.message);
          } else {
            this.$message.success("操作成功");
            this.getPermissions();
          }
        })
        .catch(() => {});
    },
    addPermission() {
      this.$refs.formRef.validate(async valid => {
        if (!valid) return;
        this.form.type = parseInt(this.form.type);
        const { data: result } = await this.$http.post(
          "/permissions",
          this.form
        );
        if (result.code == 0) {
          this.$message.success("操作成功");
          this.addDialogFormVisible = false;
          this.getPermissions();
        }
      });
    },
    async modifyPermission() {
      this.form.type = parseInt(this.form.type);
      const { data: result } = await this.$http.put(
        `permission/${this.form.id}`,
        this.form
      );
      if (result.code != 0) {
        this.$message.error(result.message);
      } else {
        this.$message.success("操作成功");
        this.modifyDialogFormVisible = false;
        this.getPermissions();
      }
    },
    editPermission() {},
    async getPermissions() {
      const { data: result } = await this.$http.get("permissions");
      if (result.code != 0) {
        this.$message.error(result.message);
      } else {
        this.data = result.data;
      }
    },
    ifEnd(data){
        return data.children==null;
    }
  },

  data() {
    return {
      filterText: "",
      form: {
        name: "",
        path: "",
        code: "",
        type: 0,
        remarks: ""
      },
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
      data: [],
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