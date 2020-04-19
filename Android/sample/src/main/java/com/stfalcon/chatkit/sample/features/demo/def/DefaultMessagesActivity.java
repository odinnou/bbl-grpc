package com.stfalcon.chatkit.sample.features.demo.def;

import android.annotation.SuppressLint;
import android.app.Activity;
import android.content.Context;
import android.content.Intent;
import android.os.AsyncTask;
import android.os.Bundle;
import android.text.TextUtils;
import android.util.Log;
import android.view.View;

import com.stfalcon.chatkit.messages.MessageInput;
import com.stfalcon.chatkit.messages.MessagesList;
import com.stfalcon.chatkit.messages.MessagesListAdapter;
import com.stfalcon.chatkit.sample.R;
import com.stfalcon.chatkit.sample.common.data.fixtures.MessagesFixtures;
import com.stfalcon.chatkit.sample.common.data.model.Message;
import com.stfalcon.chatkit.sample.common.data.model.User;
import com.stfalcon.chatkit.sample.features.demo.DemoMessagesActivity;
import com.stfalcon.chatkit.sample.utils.AppUtils;

import java.lang.ref.WeakReference;
import java.util.Comparator;
import java.util.Date;
import java.util.List;
import java.util.UUID;
import java.util.concurrent.CountDownLatch;
import java.util.concurrent.TimeUnit;
import java.util.stream.Collectors;

import Server.ChatOuterClass;
import io.grpc.ManagedChannel;
import io.grpc.ManagedChannelBuilder;
import io.grpc.stub.StreamObserver;

public class DefaultMessagesActivity extends DemoMessagesActivity
        implements MessageInput.InputListener,
        MessageInput.TypingListener {

    public static void open(Context context) {
        context.startActivity(new Intent(context, DefaultMessagesActivity.class));
    }

    private MessagesList messagesList;
    private ManagedChannel channel;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_default_messages);

        this.messagesList = (MessagesList) findViewById(R.id.messagesList);

        channel = ManagedChannelBuilder.forAddress("192.168.0.25", 47001).usePlaintext().build();

        initAdapter();

        MessageInput input = (MessageInput) findViewById(R.id.input);
        input.setInputListener(this);
        input.setTypingListener(this);
    }

    private   StreamObserver<ChatOuterClass.PostMessageRequest> requestObserver;

    private void initChat() {

        Server.ChatGrpc.ChatStub asyncStub = Server.ChatGrpc.newStub(channel);
       requestObserver=   asyncStub.participate(new StreamObserver<ChatOuterClass.MessageResponse>() {
                   @Override
                   public void onNext(ChatOuterClass.MessageResponse note) {

                       DefaultMessagesActivity.this.runOnUiThread(new Runnable() {
                           public void run() {
                               addMessage(note);
                           }
                       });
                   }

                   @Override
                   public void onError(Throwable t) {
                       t.printStackTrace();
                   }

                   @Override
                   public void onCompleted() {
                   }
               });

        ChatOuterClass.PostMessageRequest request =  ChatOuterClass.PostMessageRequest.newBuilder()
                .setLogin("Android")
                .build();

        requestObserver.onNext(request);
    }

    private static class GrpcTask extends AsyncTask< ManagedChannel, Void, List<ChatOuterClass.MessageResponse>> {
        private final WeakReference<DefaultMessagesActivity> activityReference;

        private GrpcTask(DefaultMessagesActivity activity) {
            this.activityReference = new WeakReference<DefaultMessagesActivity>(activity);
        }

        @SuppressLint("NewApi")
        @Override
        protected List<ChatOuterClass.MessageResponse> doInBackground( ManagedChannel... params) {
            try {
                Server.ChatGrpc.ChatBlockingStub stub = Server.ChatGrpc.newBlockingStub(params[0]);
                ChatOuterClass.GetMessagesRequest request = ChatOuterClass.GetMessagesRequest.newBuilder()
                                                                                    .setLastMessages(100)
                                                                                    .build();
                ChatOuterClass.GetMessagesResponse response = stub.getHistory(request);
                return response.getMessagesList()
                        .stream().sorted((f1, f2) -> Long.compare(f1.getDateCreated().getSeconds(), f2.getDateCreated().getSeconds()))
                        .collect(Collectors.toList());
            } catch (Exception e) {
                return null;
            }
        }

        @Override
        protected void onPostExecute(List<ChatOuterClass.MessageResponse> result) {
            DefaultMessagesActivity activity = activityReference.get();
            if (activity == null) {
                return;
            }

            for (ChatOuterClass.MessageResponse message :result) {
                activity.addMessage(message);
            }

            activity.initChat();
        }
    }

    public void addMessage(ChatOuterClass.MessageResponse messageResponse)
    {
        User user = new User(messageResponse.getLogin().equals("Android")?"0":"1", messageResponse.getLogin(),"",true);
        Message message = new Message(Long.toString(UUID.randomUUID().getLeastSignificantBits()),user,messageResponse.getMessage(),new Date(messageResponse.getDateCreated().getSeconds()));

        super.messagesAdapter.addToStart(message, true);
    }

    @Override
    public boolean onSubmit(CharSequence input) {



        try {

            ChatOuterClass.PostMessageRequest request =  ChatOuterClass.PostMessageRequest.newBuilder()
                    .setLogin("Android")
                    .setMessage(input.toString())
                    .build();

                requestObserver.onNext(request);

        } catch (Exception e) {
            // Cancel RPC
            requestObserver.onCompleted();
            e.printStackTrace();
        }

        return true;
    }


    private void initAdapter() {
        super.messagesAdapter = new MessagesListAdapter<>(super.senderId, super.imageLoader);
        this.messagesList.setAdapter(super.messagesAdapter);


        new GrpcTask(this)
                .execute(channel);
    }

    @Override
    public void onStartTyping() {
        Log.v("Typing listener", getString(R.string.start_typing_status));
    }

    @Override
    public void onStopTyping() {
        Log.v("Typing listener", getString(R.string.stop_typing_status));
    }
}
