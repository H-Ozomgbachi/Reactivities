import { useEffect } from "react";
import NavBar from "./NavBar";
import ActivityDashboard from "../../features/activities/dashboard/ActivityDashboard";
import LoadingComponent from "./LoadingComponent";
import { useStore } from "../stores/store";
import { observer } from "mobx-react-lite";
import { Container } from "semantic-ui-react";

const App = () => {
  const { activityStore } = useStore();

  useEffect(() => {
    activityStore.loadActivities();
  }, [activityStore]);

  if (activityStore.loadingInitial)
    return <LoadingComponent content="Loading app" inverted={true} />;

  return (
    <>
      <NavBar />

      <div style={{ marginTop: "7em" }}>
        <Container>
          <ActivityDashboard />
        </Container>
      </div>
    </>
  );
};

export default observer(App);
