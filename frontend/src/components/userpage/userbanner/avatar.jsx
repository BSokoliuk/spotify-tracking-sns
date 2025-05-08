import { createEffect, createSignal, useContext } from "solid-js";
import { UserContext } from "../../../contexts/UserContext";
import { getData, postData, deleteData, patchData } from "../../../getUserData";
import tickIcon from "../../../assets/icons/tick.svg";
import plusIcon from "../../../assets/icons/plus.svg";
import editIcon from "../../../assets/icons/edit.svg";
import { AdminContext } from "../../../contexts/AdminContext";

function Avatar(props) {
  const { user } = useContext(UserContext);
  const { admin } = useContext(AdminContext);
  const [image, setImage] = createSignal(props.image);
  const [isFollowing, setIsFollowing] = createSignal(false);
  const [newImage, setNewImage] = createSignal(null);

  const getIsFollowing = async () => {
    if (!user()) return;
    if (user().id == props.profileId) return;
    const response = await getData(`follow/${props.profileId}/is-following`);
    if (response.success) {
      setIsFollowing(response.isFollowing);
    }
  };

  createEffect(() => {
    getIsFollowing();
  });

  createEffect(() => {
    setImage(props.image);
  });

  const handleClick = async (e) => {
    e.preventDefault();
    if (isFollowing()) {
      const response = await deleteData(`follow/${props.profileId}`);
      if (response.success) {
        setIsFollowing(false);
      }
    } else {
      const response = await postData(`follow/${props.profileId}`);
      if (response.success) {
        setIsFollowing(true);
      }
    }
  };

  createEffect(() => {
    if (newImage()) handleEditAvatar();
  });

  const onUpload = (e) => {
    e.preventDefault();
    let file = e.target.files[0];
    if (file) {
      let reader = new FileReader();
      reader.onloadend = () => {
        setNewImage(reader.result);
      };
      reader.readAsDataURL(file);
    }
  };

  const handleEditAvatar = async () => {
    const res = await patchData("admin/avatar", {
      avatar: newImage(),
      userId: props.profileId,
    });
    if (res.success) {
      //FIXME: response is no longer returning avatar
      // requires a refresh page
      window.location.reload();
      setImage(res.avatar);
    }
    setNewImage(null);
  };

  return (
    <div
      class="h-[100%] aspect-square border border-[#3f4147] relative bg-no-repeat bg-cover"
      style={`background-image: url(data:image/png;base64,${image()})`}
    >
      {user() && user().id != props.profileId && (
        <div>
          {admin() && (
            <>
              <button
                class="absolute top-0 right-0 rounded-lg cursor-pointer hover:opacity-90 transition-all duration-150 bg-slate-400 opacity-60"
                onClick={(e) => {
                  e.preventDefault();
                  document.getElementById("newAvatar").click();
                }}
              >
                <img src={editIcon} class="w-6 h-6" />
              </button>
              <input
                type="file"
                id="newAvatar"
                class="hidden"
                onChange={onUpload}
              />
            </>
          )}
          <button
            class="absolute bottom-0 right-0 rounded-lg cursor-pointer hover:opacity-90 transition-all duration-150 bg-slate-400 opacity-60"
            onClick={(e) => handleClick(e)}
          >
            <img src={isFollowing() ? tickIcon : plusIcon} />
          </button>
        </div>
      )}
    </div>
  );
}

export default Avatar;
